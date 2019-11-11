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
    private int fishTime = 1;
    private bool fishingPause = false;

    [SerializeField]
    private Ship ship;

    [SerializeField]
    private int timeAlive;
    [SerializeField]
    private int distanceTravelled;

    [SerializeField]
    private FishPool fishPool;
    [SerializeField]
    private CrewGenerator crewGenerator;
    private ProfileSO crewCandidate;

    [SerializeField]
    private GameOver gameOverScreen;
    private bool gameOver = false;
    public enum GameOverReason : byte {
        Altitude,
        Mutiny
    }

    [SerializeField]
    private Parallax background;

    [SerializeField]
    private DecisionUI decisionUI;

    void Start() {
        crewGenerator.calcProbabilities();
        nextTick = Time.time;
    }

    void Update() {
        timeAlive = ship.ticksSurvived;
        distanceTravelled = ship.distanceTravelled;

        if (!gameOver && Time.time >= nextTick) {
            HandleFishing();
            nextTick = Time.time + tickLength / fastForward;
        }

        if (ship.air == 0) {
            EndGame(GameOverReason.Altitude);
        }
    }

    public int CalculateScore() {
        return timeAlive;
    }

    public void EndGame(GameOverReason reason) {
        gameOver = true;
        switch (reason) {
            case GameOverReason.Altitude:
                gameOverScreen.gameOverReason = "Like Icarus, you fell from the sky, except with more swag. Hopefully.";
                break;
            case GameOverReason.Mutiny:
                gameOverScreen.gameOverReason = "Your crew decide to give you a taste of your own medicine and tossed you overboard";
                break;
            default:
                gameOverScreen.gameOverReason = "You managed to lose in a way that the developers didn't even know was possible!";
                break;
        }
        gameOverScreen.timeAlive = timeAlive;
        gameOverScreen.finalScore = CalculateScore();
        gameOverScreen.gameObject.SetActive(true);
        background.Speed = 0;
    }

    void HandleFishing() {
        if (!fishingPause) {
            ++fishClock;
        }
        if (fishClock >= fishTime) {
            Debug.Log("Fished");
            fishingPause = true;
            ItemSO item = fishPool.FishItem();
            if (item.category.Equals(ItemSO.ItemCategory.Crew)) {
                crewCandidate = crewGenerator.GenerateCrewProfile(item);
                decisionUI.setUpProfile(crewCandidate);
            } else {
                decisionUI.setUpItem(item);
            }
            fishClock = 0;
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
        Debug.Log("Refused");
    }

    public void AddCrew(int pos) {
        ship.BootCrew(pos);
        ship.AdoptCrew(crewCandidate, pos);
        fishPool.CatchItem();
        crewCandidate = null;
    }
}
