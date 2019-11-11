using UnityEngine;
using static ItemGenerator;

[CreateAssetMenu(fileName = "MyItem", menuName = "ScriptableObjects/Item", order = 1)]
public class ItemSO : ScriptableObject
{
    public enum ItemCategory : byte {
        Unique,
        Junk,
        Alcohol,
        Fuel,
        Air,
        Crew,
        Flair
    }
    public string itemName = "<name>";
    public string flavorText = "<flavor>";
    public ItemCategory category = ItemCategory.Unique;
    public float rarity = 0.01f;
    public int quantity = 1;
    public float weight = 0;

    public override string ToString()
    {
        return string.Format("{4}-{0}:\n\t\"{2}\"\n\tRarity: {5}\n\tWeight: {1}kg\n\tQuantity: x{3}", itemName, weight, flavorText, quantity, category, rarity);
    }
}

