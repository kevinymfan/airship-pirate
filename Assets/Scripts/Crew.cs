using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crew : MonoBehaviour {
    public ProfileSO profile;

    [SerializeField]
    private Ship ship;

    public int happiness;
    private int maxHappiness = 8;
    private int happinessClock = 0;
    [SerializeField]
    private int happinessIncreaseTime = 3;

    public int drunkenness;
    [SerializeField]
    private float maxDrunkenness = 16f;
    int drinkingClock = 0;
    [SerializeField]
    int recoveryCooldown = 10;
    [SerializeField]
    int recoveryTime = 5;
    [SerializeField]
    int drinkingCooldown = 5;

    float nextTick;
    [SerializeField]
    private float tickLength = 1f;

    void Start() {
        nextTick = Time.time;
        happiness = Random.Range(5, 8);
        drunkenness = 0;
    }

    public void Reset() {
        nextTick = Time.time;
        happiness = 6;
        drunkenness = 0;
    }

    void Update() {
        if (Time.time >= nextTick) {
            HandleDrinking();
            HandleHappiness();
            nextTick = Time.time + tickLength;
        }
    }

    private void HandleDrinking() {
        ++drinkingClock;
        if (happiness < 5 && drinkingClock > drinkingCooldown) {
            float alcoholServing = ship.ServeAlcohol();
            drunkenness = Mathf.RoundToInt(Mathf.Min(drunkenness + alcoholServing * maxDrunkenness / profile.tolerance, maxDrunkenness));
            drinkingClock = 0;
            return;
        }

        if (drinkingClock > recoveryCooldown) {
            drunkenness = Mathf.Max(0, drunkenness - Mathf.RoundToInt(maxDrunkenness / 8));
            drinkingClock -= recoveryTime;
        }
    }

    private void HandleHappiness() {
        ++happinessClock;

        float normalizedDrunkenness = drunkenness / maxDrunkenness;
        if (normalizedDrunkenness > 0.5 && happiness < maxHappiness) {
            int threshold = happinessIncreaseTime;
            if (normalizedDrunkenness < .8) {
                threshold = happinessIncreaseTime * 2;
            }
            if (happinessClock > threshold) {
                happiness += 1;
                happinessClock = 0;
            }
        } else if (normalizedDrunkenness < .1 && happiness > 0) {
            if (happinessClock > profile.temperment) {
                happiness -= 1;
                happinessClock = 0;
            }
        }
    }

    public void ChangeHappiness(int amount) {
        happiness = Mathf.Clamp(happiness + amount, 0, 8);
    }

    public ProfileSO.SkillType GetSkillType() {
        return profile.skillType;
    }

    public int GetSkillLevel() {
        if (profile.skillType == ProfileSO.SkillType.None) {
            return 0;
        }

        switch (profile.skillLevel) {
            case ProfileSO.SkillLevel.Amateur:
                return 1;
            case ProfileSO.SkillLevel.Adept:
                return 2;
            case ProfileSO.SkillLevel.Master:
                return 3;
            default:
                return 0;
        }
    }
}
