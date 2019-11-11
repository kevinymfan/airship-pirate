using System.Collections;
using System.Collections.Generic;
using static ItemSO;
using UnityEngine;

[System.Serializable]
public class ItemGenerator
{
    const int maxMax = 100;
    const int defaultMax = 50;
    [SerializeField, Range(1, maxMax)]
    private int maxFuelQ = defaultMax;
    [SerializeField, Range(1, maxMax)]
    private int maxAirQ = defaultMax;
    [SerializeField, Range(1, maxMax)]
    private int maxAlcoholQ = defaultMax;
    [SerializeField, Range(0.5f, 10.0f)]
    private float maxClothFlairW = 5.0f;
    [SerializeField, Range(2.5f, 50.0f)]
    private float maxShipFlairW = 50.0f;
    private RarityText junkText;
    private RarityText alcoholText;
    private RarityText fuelText;
    private RarityText airText;
    private RarityText flairText;
    [HideInInspector]
    public FishPool pool;

    public void Start() {
        junkText = RarityText.CreateFromJSON(Resources.Load<TextAsset>("itemText/junkText").ToString());
        alcoholText = RarityText.CreateFromJSON(Resources.Load<TextAsset>("itemText/alcoholText").ToString());
        fuelText = RarityText.CreateFromJSON(Resources.Load<TextAsset>("itemText/fuelText").ToString());
        airText = RarityText.CreateFromJSON(Resources.Load<TextAsset>("itemText/airText").ToString());
        flairText = RarityText.CreateFromJSON(Resources.Load<TextAsset>("itemText/flairText").ToString());
    }

    public ItemSO GenerateItem(ItemCategory category, float rarity) {
        ItemSO item = this.GenerateItemSwitch(category, rarity);
        item.category = category;
        Debug.Log("Random Item Generated: " + item);
        return item;
    }
    private ItemSO GenerateItemSwitch(ItemCategory cat, float rarity) {
        rarity = Mathf.Clamp(rarity, 0, 0.9995f);
        Debug.Log(string.Format("=== Generating item ===\nCategory: {0}   Rarity: {1}", cat, rarity));
        switch(cat) {
            case ItemCategory.Junk: return generateJunk(rarity);
            case ItemCategory.Alcohol: return generateAlcohol(rarity);
            case ItemCategory.Fuel: return generateFuel(rarity);
            case ItemCategory.Air: return generateAir(rarity);
            case ItemCategory.Crew: return generateCrew(rarity);
            case ItemCategory.Flair: return generateFlair(rarity);
            case ItemCategory.Unique:
            default: break;
        }
        return null;
    }

    private void setItem(ItemSO item, RarityText text, float rarity) {
        int bucket = (int) (text.names.Length * rarity);
        int choice = 0;
        // choose item name
        choice = (int) ((text.names[bucket].choices.Length + text.universalNames.Length) * rarity);
        if (choice < text.names[bucket].choices.Length) {
            item.itemName = text.names[bucket].choices[choice];
        }
        else {
            item.itemName = text.universalNames[choice - text.names[bucket].choices.Length];
        }
        // choose item flavor text
        choice = (int) ((text.flavors[bucket].choices.Length + text.universalFlavors.Length) * rarity);
        if (choice < text.flavors[bucket].choices.Length) {
            item.flavorText = text.flavors[bucket].choices[choice];
        }
        else {
            item.flavorText = text.universalFlavors[choice - text.flavors[bucket].choices.Length];
        }
    }

    private ItemSO generateJunk(float rarity) {
        ItemSO item = ScriptableObject.CreateInstance<ItemSO>();
        this.setItem(item, this.junkText, rarity);
        return item;
    }
    private ItemSO generateAlcohol(float rarity) {
        ItemSO item = ScriptableObject.CreateInstance<ItemSO>();
        this.setItem(item, this.alcoholText, rarity);
        item.quantity = Mathf.Clamp((int) (this.maxAlcoholQ * rarity) + 1, 1, this.maxAlcoholQ);
        return item;
    }
    private ItemSO generateFuel(float rarity) {
        ItemSO item = ScriptableObject.CreateInstance<ItemSO>();
        this.setItem(item, this.fuelText, rarity);
        item.quantity = Mathf.Clamp((int) (this.maxFuelQ * rarity) + 1, 1, this.maxFuelQ);
        return item;
    }
    private ItemSO generateAir(float rarity) {
        ItemSO item = ScriptableObject.CreateInstance<ItemSO>();
        this.setItem(item, this.airText, rarity);
        item.quantity = Mathf.Clamp((int) (this.maxAirQ * rarity) + 1, 1, this.maxAirQ);
        return item;
    }
    private ItemSO generateCrew(float rarity) {
        ItemSO item = ScriptableObject.CreateInstance<ItemSO>();
        item.rarity = rarity;
        return item;
    }
    private ItemSO generateFlair(float rarity) {
        ItemSO item = ScriptableObject.CreateInstance<ItemSO>();
        bool flipW = Random.value > 0.5f;
        // Cloth or Ship flair
        int flair = Random.Range(0, 2);
        item.rarity= rarity;
        item.weight = flipW ? 1 - rarity : rarity;
        switch(flair) {
            case 0: this.setClothFlair(item, rarity, flipW); break;
            case 1: this.setShipFlair(item, rarity, flipW); break;
            default: break;
        }
        return item;
    }

    private void setClothFlair(ItemSO item, float rarity, bool flipW) {
        item.weight *= this.maxClothFlairW;
        item.itemName = "STYLISH CLOTH";
        
    }

    private void setShipFlair(ItemSO item, float rarity, bool flipW) {
        item.weight *= this.maxShipFlairW;
        item.itemName = "STYLISH SHIP PART";
    }

}
