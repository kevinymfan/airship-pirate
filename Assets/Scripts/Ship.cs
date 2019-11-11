using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour {
    float nextTick;
    [SerializeField]
    private float tickLength = 1f;

    public float fuel = 100;
    [SerializeField]
    private float maxFuel = 100;
    [SerializeField]
    private int fuelUseTime = 5;
    private int fuelClock = 0;

    public float air = 100;
    [SerializeField]
    private float maxAir = 100;
    public float baseAirLossRate = -1;
    [SerializeField]
    private float airGainBase = 0.5f;
    [SerializeField]
    private int airLossTime = 3;
    private int airClock = 0;

    public float alcohol = 0f;
    [SerializeField]
    private float maxAlcohol = 100f;

    [SerializeField]
    private int speed = 10;
    public int distanceTravelled = 0;

    [SerializeField]
    Crew[] crewList = new Crew[5];
    Dictionary<ProfileSO.SkillType, int> crewSkills = new Dictionary<ProfileSO.SkillType, int>();

    void Start() {
        crewSkills[ProfileSO.SkillType.Engine] = 0;
        crewSkills[ProfileSO.SkillType.Blower] = 0;
        nextTick = Time.time;
    }

    void Update() {
        if (Time.time >= nextTick) {
            PassTime();
            nextTick = Time.time + tickLength;
        }
    }

    private int GetEmptyCrewSlot() {
        for (int i = 0; i < crewList.Length; i++) {
            if (!crewList[i].isActiveAndEnabled) {
                return i;
            }
        }
        return -1;
    }

    public bool CanAdoptCrew() {
        return GetEmptyCrewSlot() != -1;
    }

    public void AdoptCrew(ProfileSO newMember) {
        int crewSlot = GetEmptyCrewSlot();
        if (crewSlot == -1) {
            Debug.Log("Tried to adopt crew member, but no slots were available");
            return;
        }
        crewList[crewSlot].profile = newMember;
        if (crewList[crewSlot].GetSkillLevel() > 0) {
            crewSkills[crewList[crewSlot].GetSkillType()] += crewList[crewSlot].GetSkillLevel();
        }
        crewList[crewSlot].gameObject.SetActive(true);
    }

    public void BootCrew(int crewSlot) {
        crewList[crewSlot].gameObject.SetActive(false);
        if (crewList[crewSlot].GetSkillLevel() > 0) {
            crewSkills[crewList[crewSlot].GetSkillType()] -= crewList[crewSlot].GetSkillLevel();
        }
    }

    public int GetSpeed() {
        return speed + crewSkills[ProfileSO.SkillType.Engine];
    }

    private float GetAirRate() {
        return baseAirLossRate + crewSkills[ProfileSO.SkillType.Blower] * airGainBase;
    }

    private void PassTime() {
        distanceTravelled += GetSpeed();

        ++fuelClock;
        if (fuelClock > fuelUseTime && fuel > 0) {
            fuel -= 1;
            fuelClock = 0;
        }

        ++airClock;
        if (airClock > airLossTime) {
            air = Mathf.Clamp(air + GetAirRate(), 0, maxAir);
            airClock = 0;
        }
    }

    public float ServeAlcohol() {
        if (alcohol < 0.5) {
            return 0f;
        }

        float serving = Random.Range(0.5f, Mathf.Min(alcohol, 2f));
        alcohol -= serving;
        return serving;
    }

    public void GainAlcohol(float amount) {
        alcohol = Mathf.Min(alcohol + amount, maxAlcohol);
    }

    public void GainFuel(float amount) {
        fuel = Mathf.Min(fuel + amount, maxFuel);
    }

    public void GainAir(float amount) {
        air = Mathf.Min(air + amount, maxAir);
    }

    public int GetAirLevel() {
        return Mathf.RoundToInt(air);
    }
}
