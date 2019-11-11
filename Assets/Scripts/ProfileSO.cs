using UnityEngine;

[CreateAssetMenu(fileName = "MyProfile", menuName = "ScriptableObjects/Profile", order = 1)]
public class ProfileSO : ScriptableObject {
    public string crewName = "<name>";
    public string flavorText = "<flavor>";
    public float weight = 0;
    public int tolerance = 1;
    public float temperment = 1;
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
        return string.Format("{0} ({1}kg): {5} {6}\n\t\"{2}\"\n\tTolerance: {3}\n\tTemperment: {4}", crewName, weight, flavorText, tolerance, temperment, skillType, skillLevel);
    }
}

