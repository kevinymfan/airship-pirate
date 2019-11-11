using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RarityText
{
    public PotentialChoices[] names;
    public string[] universalNames;
    public PotentialChoices[] flavors;
    public string[] universalFlavors;

    public static RarityText CreateFromJSON(string jsonString) {
        return JsonUtility.FromJson<RarityText>(jsonString);
    }

    public static string ToJSON(RarityText obj) {
        return JsonUtility.ToJson(obj);
    }
}
[System.Serializable]
public class PotentialChoices {
    public string[] choices;

    public PotentialChoices(string[] vals) {
        this.choices = vals;
    }
}