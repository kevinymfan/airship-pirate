using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CrewGenerator
{

    private int maxTol;
    [SerializeField, Range(30, 100)]
    private int minW = 30;
    [SerializeField, Min(100)]
    private int maxW = 100;
    [SerializeField, Range(0, 1)]
    private float rarityWAdjust = 0.2f;
    [SerializeField, Min(1)]
    private int maxTempRate = 3;
    [SerializeField]
    private ProfileSO.SkillType[] skillTypes = new ProfileSO.SkillType[] {
        ProfileSO.SkillType.Blower,
        ProfileSO.SkillType.Engine,
        ProfileSO.SkillType.Lookout
    };
    [SerializeField]
     private int[] skillTypesPropabilities = new int[] { 1, 1, 1 };
    private float[] skillTypeProps;
    [SerializeField]
    private ProfileSO.SkillLevel[] skillLevels = new ProfileSO.SkillLevel[] {
        ProfileSO.SkillLevel.Amateur,
        ProfileSO.SkillLevel.Adept,
        ProfileSO.SkillLevel.Master
    };
    [SerializeField]
    private int[] skillLevelsPropabilities = new int[] { 1, 1, 1 };
    private float[] levelProps;
    public CrewGenerator()
    {
        this.maxTol = 16;
    }

    public void calcProbabilities()
    {
        // Calc level probabilities
        this.levelProps = new float[this.skillLevelsPropabilities.Length];
        int levelSum = 0;
        for (int i = 0; i < this.skillLevelsPropabilities.Length; ++i)
        {
            this.levelProps[i] = this.skillLevelsPropabilities[i];
            levelSum += this.skillLevelsPropabilities[i];
        }
        for (int i = 0; i < this.levelProps.Length; ++i)
        {
            this.levelProps[i] /= levelSum;
        }
        // Calc type probabilities
        this.skillTypeProps = new float[this.skillLevelsPropabilities.Length];
        int skillSum = 0;
        for (int i = 0; i < this.skillTypesPropabilities.Length; ++i)
        {
            this.skillTypeProps[i] = this.skillTypesPropabilities[i];
            skillSum += this.skillTypesPropabilities[i];
        }
        for (int i = 0; i < this.skillTypeProps.Length; ++i)
        {
            this.skillTypeProps[i] /= skillSum;
        }
    }

    public ProfileSO GenerateCrewProfile(ItemSO crewItem)
    {
        if (!crewItem.category.Equals(ItemSO.ItemCategory.Crew))
        {
            return null;
        }
        float rarity = crewItem.rarity;
        ProfileSO profile = ScriptableObject.CreateInstance<ProfileSO>();
        profile.weight = this.generateWeight(rarity);
        profile.tolerance = this.generateTolerance(rarity);
        profile.temperment = this.generateTemperance(rarity);
        profile.skillType = this.generateSkillType(rarity);
        profile.skillLevel = this.generateSkillLevel(rarity);
        Debug.Log("=== Generating Crew Profile ===\n\tFrom item: " + crewItem + "\n\tCreated: " + profile);
        return profile;
    }

    private int generateTolerance(float rarity)
    {
        int tol = Mathf.Clamp((int)((this.maxTol - 1) * Random.value) + 1, 1, this.maxTol);
        return tol;
    }

    private float generateTemperance(float rarity)
    {
        float temp = rarity * (this.maxTempRate - 1f) + 1f;
        return temp;
    }

    private ProfileSO.SkillType generateSkillType(float rarity)
    {
        float choiceVal = Random.value;
        float sum = 0;
        int choice = 0;
        for (int c = 0; c < this.skillTypeProps.Length; ++c) {
            sum += this.skillTypeProps[c];
            if (sum > choiceVal) {
                choice = c;
                break;
            }
        }
        return this.skillTypes[choice];
    }

    private ProfileSO.SkillLevel generateSkillLevel(float rarity)
    {
        float choiceVal = rarity;
        float sum = 0;
        int choice = 0;
        for (int c = 0; c < this.levelProps.Length; ++c) {
            sum += this.levelProps[c];
            if (sum > choiceVal) {                
                choice = c;
                break;
            }
        }
        return this.skillLevels[choice];
    }

    private float generateWeight(float rarity)
    {
        float weightVal = Mathf.Clamp(Random.value - rarity * this.rarityWAdjust, 0, 1f);
        float weight = ((this.maxW - this.minW) * weightVal) + this.minW;
        return weight;
    }
}