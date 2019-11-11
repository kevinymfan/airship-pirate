using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crew : MonoBehaviour {
    public ProfileSO profile;

    private Ship ship;

    int happiness;
    int drunkenness;

    int happinessClock = 0;
    int drinkingClock = 0;

    [SerializeField]
    int recoveryCooldown = 100;
    [SerializeField]
    int recoveryTime = 10;
    [SerializeField]
    int drinkingCooldown = 30;

    const int maxDrunkenness = 8;
    const int maxHappiness = 8;
    
    void Start() {
        happiness = 6;
        drunkenness = 0;
    }

    void Update() {
        
    }

    private void HandleDrinking() {
        ++drinkingClock;
        if (happiness < 3 && drinkingClock > drinkingCooldown) {
            if (ship.DrinkAlcohol(profile.tolerance)) {
                drinkingClock = 0;
                drunkenness += 1;
                return;
            }
        }

        if (drinkingClock > recoveryCooldown) {
            drunkenness -= 1;
            drinkingClock -= recoveryTime;
        }
    }

    private void HandleHappiness() {
        ++happinessClock;

        if (drunkenness > 4 && happiness < maxHappiness) {
            int threshold = 10;
            if (drunkenness < 7) {
                threshold = 20;
            }
            if (happinessClock > threshold) {
                happiness += 1;
                happinessClock = 0;
            }
        } else if (drunkenness <= 1 && happiness > 0) {
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
