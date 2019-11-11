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
    private RarityText crewText;
    public CrewGenerator()
    {
        this.maxTol = 16;
    }

    public void calcProbabilities()
    {
        // Shhh.. I shouldn't call this here
        crewText = RarityText.CreateFromJSON(Resources.Load<TextAsset>("itemText/crewText").ToString());
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
        for (int c = 0; c < this.skillTypeProps.Length; ++c)
        {
            sum += this.skillTypeProps[c];
            if (sum > choiceVal)
            {
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
        for (int c = 0; c < this.levelProps.Length; ++c)
        {
            sum += this.levelProps[c];
            if (sum > choiceVal)
            {
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

    private string myPronouns(bool male)
    {
        return male ? "he" : "she";
    }

    private void setText(ProfileSO profile, RarityText text, float rarity)
    {
        bool male = Random.value > 0.5f;
        int bucket = male ? 0 : 1;
        int choice = 0;
        // choose item name
        choice = (int)((text.names[bucket].choices.Length + text.universalNames.Length) * rarity);
        if (choice < text.names[bucket].choices.Length)
        {
            profile.crewName = text.names[bucket].choices[choice];
        }
        else
        {
            profile.crewName = text.universalNames[choice - text.names[bucket].choices.Length];
        }
        // Decide temperment text
        string temperment;
        if (profile.temperment < this.maxTempRate * 0.3)
        {
            temperment = string.Format("Looks like {0}'s about to cry.", myPronouns(male));
        }
        else if (profile.temperment < this.maxTempRate * 0.75)
        {
            temperment = string.Format("Looks like {0} could easily complete a days work.", myPronouns(male));
        }
        else
        {
            temperment = string.Format("{0} looks quite cheery.", myPronouns(male));
        }
        // Decide tolerence text
        string tolerance;
        if (profile.tolerance < this.maxTol * 0.3)
        {
            tolerance = string.Format("A drink or two and this crewmate is high as the crow's nest!", myPronouns(male));
        }
        else if (profile.tolerance < this.maxTol * 0.75)
        {
            tolerance = string.Format("", myPronouns(male));
        }
        else
        {
            tolerance = string.Format("Don't get in a drinking bout with this fellow!", myPronouns(male));
        }
        // Decide weight text
        string weight;
        if (profile.weight < this.maxTempRate * 0.3)
        {
            weight = string.Format("{0} has worked {1} to the bone.", myPronouns(male), male ? "himself" : "herself");
        }
        else if (profile.weight < this.maxTempRate * 0.75)
        {
            weight = "";
        }
        else
        {
            weight = string.Format("Remember, don't assign them to climb the rigging.", myPronouns(male));
        }

        profile.flavorText = string.Format("An {4} {3}. {0} {1} {2}", temperment, weight, tolerance, profile.skillType, profile.skillLevel);

        // choose item flavor text
        // choice = (int)((text.flavors[bucket].choices.Length + text.universalFlavors.Length) * rarity);
        // if (choice < text.flavors[bucket].choices.Length)
        // {
        //     item.flavorText = text.flavors[bucket].choices[choice];
        // }
        // else
        // {
        //     item.flavorText = text.universalFlavors[choice - text.flavors[bucket].choices.Length];
        // }
    }
}