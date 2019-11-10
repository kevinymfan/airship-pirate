using UnityEngine;

[CreateAssetMenu(fileName = "MyItem", menuName = "ScriptableObjects/Item", order = 1)]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public string flavorText;
    public float weight;

    public override string ToString()
    {
        return string.Format("{0} ({1}kg): \"{2}\"", itemName, weight, flavorText);
    }
}

