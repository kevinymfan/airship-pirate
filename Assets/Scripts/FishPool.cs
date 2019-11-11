using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishPool : MonoBehaviour
{
    [SerializeField]
    private ItemSO[] firstFish;
    [SerializeField]
    private Ship ship;
    [SerializeField]
    private AnimationCurve rarityDistribution;

    private Queue<ItemSO> firstFishes;
    [SerializeField]
    private ItemSO.ItemCategory[] categories = new ItemSO.ItemCategory[] {
        ItemSO.ItemCategory.Junk,
        ItemSO.ItemCategory.Alcohol,
        ItemSO.ItemCategory.Air,
        ItemSO.ItemCategory.Fuel,
        ItemSO.ItemCategory.Crew,
        ItemSO.ItemCategory.Flair
    };
    [SerializeField]
    private ItemGenerator itemGenerator;

    [HideInInspector]
    public ItemSO fishedItem;

    private void Start()
    {
        this.firstFishes = new Queue<ItemSO>(this.firstFish);
        itemGenerator.Start();
        itemGenerator.pool = this;
    }

    public ItemSO CatchItem()
    {
        ItemSO item = this.fishedItem;
        this.fishedItem = null;
        return item;
    }

    public void ReleaseItem()
    {
        this.fishedItem = null;
    }

    public ItemSO FishItem()
    {
        if (this.fishedItem) { }
        else if (this.firstFishes.Count > 0)
        {
            this.fishedItem = this.firstFishes.Dequeue();
        }
        else
        {
            this.fishedItem = this.getRandomItem();
        }
        return this.fishedItem;
    }

    private ItemSO getRandomItem()
    {
        ItemSO item = this.itemGenerator.GenerateItem(this.getRandomCategory(), this.getRandomRarity());
        return item;
    }

    private ItemSO.ItemCategory getRandomCategory()
    {
        int choice = Mathf.Clamp((int)(this.categories.Length * Random.value), 0, this.categories.Length - 1);
        return this.categories[choice];
    }

    private float getRandomRarity()
    {
        float modifier = this.ship ? this.ship.GetRarityBoost(): 0;
        float rarity = this.rarityDistribution.Evaluate(Random.value + modifier);        
        return rarity;
    }
}
