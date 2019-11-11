using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    float nextTick;
    public float tickLength = 1f;
    [SerializeField, Range(1, 10)]
    private int fastForward = 1;

    private int fishClock = 0;
    [SerializeField]
    private int fishTime = 10;
    private bool fishingPause = false;

    [SerializeField]
    private Ship ship;

    [SerializeField]
    private int score;
    [SerializeField]
    private int distance;

    [SerializeField]
    private FishPool fishPool;
    [SerializeField]
    private CrewGenerator crewGenerator;
    private ProfileSO crewCandidate;

    [SerializeField]
    private GameOver gameOverScreen;
    public enum GameOverReason : byte {
        Altitude,
        Mutiny
    }

    void Start() {
        crewGenerator.calcProbabilities();
        nextTick = Time.time;
    }

    void Update() {
        score = ship.ticksSurvived;
        distance = ship.distanceTravelled;

        if (Time.time >= nextTick) {
            HandleFishing();
            nextTick = Time.time + tickLength / fastForward;
        }
    }

    public void EndGame(GameOverReason reason) {
        switch (reason) {
            case GameOverReason.Altitude:
                gameOverScreen.gameOverText = "Like Icarus, you fell from the sky, except with more swag";
                break;
            case GameOverReason.Mutiny:
                gameOverScreen.gameOverText = "Your crew decide to give you a taste of your own medicine and tossed you overboard";
                break;
            default:
                gameOverScreen.gameOverText = "You managed to lose in a way that the developers didn't even know was possible!";
                break;
        }
        gameOverScreen.gameObject.SetActive(true);
    }

    void HandleFishing() {
        if (!fishingPause) {
            ++fishClock;
        }
        if (fishClock >= fishTime) {
            fishingPause = true;
            ItemSO item = fishPool.FishItem();
            if (item.category.Equals(ItemSO.ItemCategory.Crew)) {
                crewCandidate = crewGenerator.GenerateCrewProfile(item);
                // Launch crew decision UI
            } else {
                // Launch Item decision
            }
        }
    }

    public void AcceptItem() {
        ItemSO item = fishPool.CatchItem();
        // Handle the item
    }

    public void Refuse() {
        fishingPause = false;
        crewCandidate = null;
        fishPool.ReleaseItem();
    }

    public void AddCrew(int pos) {
        ship.BootCrew(pos);
        ship.AdoptCrew(crewCandidate, pos);
        fishPool.CatchItem();
        crewCandidate = null;
    }
}
