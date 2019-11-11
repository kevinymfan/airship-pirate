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

    void Start() {
        DontDestroyOnLoad(this.gameObject);
        crewGenerator.calcProbabilities();
        nextTick = Time.time;
    }

    void Update() {
        if (ship) {
            score = ship.ticksSurvived;
            distance = ship.distanceTravelled;
        }

        if (Time.time >= nextTick) {
            HandleFishing();
            nextTick = Time.time + tickLength / fastForward;
        }
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
