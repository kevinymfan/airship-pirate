using UnityEngine;

[CreateAssetMenu(fileName = "MyProfile", menuName = "ScriptableObjects/Profile", order = 1)]
public class ProfileSO : ScriptableObject {
    public string crewName;
    public string flavorText;
    public float weight;
    public int tolerance;
    public int temperment;
    public SkillType skillType;
    public SkillLevel skillLevel;

    public enum SkillType : byte {
        None,
        Engine,
        Blower,
        Lookout,
        Repair,
        Jester
    }

    public enum SkillLevel : byte {
        Amateur,
        Adept,
        Master
    }

    public override string ToString() {
        return string.Format("{0} ({1}kg): \"{2}\" {3} {4} {5} {6}", crewName, weight, flavorText, tolerance, temperment, skillType, skillLevel);
    }
}

