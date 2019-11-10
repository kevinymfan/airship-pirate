using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishPool : MonoBehaviour
{
    [SerializeField]
    private ItemSO[] firstFish;
    private Queue<ItemSO> firstFishes;
    [SerializeField]
    private ItemSO[] pool;

    [HideInInspector]
    public ItemSO fishedItem;

    private void Start() {
        this.firstFishes = new Queue<ItemSO>(this.firstFish);
    }

    public ItemSO catchItem() {
        ItemSO item = this.fishedItem;
        this.fishedItem = null;
        return item;
    }

    public void releaseItem() {
        this.fishedItem = null;
    }

    public ItemSO fishItem() {
        if (this.fishedItem) { }
        else if (this.firstFishes.Count > 0) {
            this.fishedItem = this.firstFishes.Dequeue();
        }
        else {
            this.fishedItem = this.getRandomItem();
        }
        return this.fishedItem;
    }

    private ItemSO getRandomItem() {
        ItemSO item = ScriptableObject.CreateInstance<ItemSO>();
        return item;
    }
}
