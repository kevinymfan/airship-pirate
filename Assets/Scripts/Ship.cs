using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour {

    [SerializeField]
    int fuel;
    [SerializeField]
    int maxFuel;
    [SerializeField]
    int fuelUseRate;
    [SerializeField]
    int air;
    [SerializeField]
    int maxAir;
    [SerializeField]
    int airLossRate;
    [SerializeField]
    int alcohol;
    [SerializeField]
    int maxAlcohol;

    int speed = 10;
    int distanceTravelled = 0;

    int fuelClock = 0;
    int airClock = 0;

    const int maxNumCrew = 5;
    ArrayList crewList = new ArrayList();
    Dictionary<ProfileSO.SkillType, int> crewSkills = new Dictionary<ProfileSO.SkillType, int>();

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public bool CanAdoptCrew() {
        return crewList.Count < maxNumCrew;
    }

    public void AdoptCrew(Crew newMember) {
        crewList.Add(newMember);
        if (newMember.GetSkillLevel() > 0) {
            crewSkills[newMember.GetSkillType()] += newMember.GetSkillLevel();
        }
    }

    public void BootCrew(Crew member) {
        crewList.Remove(member);
        if (member.GetSkillLevel() > 0) {
            crewSkills[member.GetSkillType()] -= member.GetSkillLevel();
        }
    }

    public int GetSpeed() {
        return speed + crewSkills[ProfileSO.SkillType.Engine];
    }

    private void PassTime() {
        distanceTravelled += GetSpeed();

        ++fuelClock;
        if (fuelClock > fuelUseRate && fuel > 0) {
            fuel -= 1;
        }

        ++airClock;
        if (airClock > airLossRate) {
            air -= 1;
        }
    }

    public int GetAirLevel() {
        return air;
    }

    public bool DrinkAlcohol(int amount) {
        if (alcohol >= amount) {
            alcohol -= amount;
            return true;
        }
        return false;
    }

    public void GainAlcohol(int amount) {
        alcohol = Mathf.Min(alcohol + amount, maxAlcohol);
    }

    public void GainFuel(int amount) {
        fuel = Mathf.Min(fuel + amount, maxFuel);
    }

    public void GainAir(int amount) {
        air = Mathf.Min(air + amount, maxAir);
    }
}
