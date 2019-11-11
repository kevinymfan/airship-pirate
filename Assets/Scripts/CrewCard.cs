using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrewCard : MonoBehaviour {
    public Image portrait;
    public Text crewName;
    [SerializeField]
    private Image[] happyBar;
    [SerializeField]
    private Image[] drunkBar;

    [SerializeField]
    private Sprite[] happinessSpriteMap;
    [SerializeField]
    private Sprite[] drunkennessSpriteMap;

    [SerializeField]
    private Crew crew;
    private bool slotEmpty = true;

    [SerializeField]
    private GameObject occupied, vacant;

    [SerializeField]
    private Sprite[] portraitSprites;

    void Update() {
        if (slotEmpty) {
            if (crew.isActiveAndEnabled) {
                crewName.text = crew.profile.crewName;
                portrait.sprite = portraitSprites[Random.Range(0, portraitSprites.Length)];
                occupied.SetActive(true);
                vacant.SetActive(false);
                slotEmpty = false;
            }
        } else {
            if (crew.isActiveAndEnabled) {
                UpdateHappiness();
                UpdateDrunkenness();
            } else {
                occupied.SetActive(false);
                vacant.SetActive(true);
                slotEmpty = true;
            }
        }
    }

    private void UpdateHappiness() {
        int selector = Mathf.FloorToInt((crew.happiness + 1f) / 2) - 1;
        for (int i = 0; i < 4; i++) {
            if (i < selector) {
                happyBar[i].sprite = happinessSpriteMap[selector*3+2];
            } else if (i > selector) {
                happyBar[i].sprite = happinessSpriteMap[selector*3];
            } else {
                if (crew.happiness % 2 == 0) {
                    happyBar[i].sprite = happinessSpriteMap[selector*3+2];
                } else {
                    happyBar[i].sprite = happinessSpriteMap[selector*3+1];
                }
            }
        }
    }

    private void UpdateDrunkenness() {
        int selector = Mathf.FloorToInt((crew.drunkenness + 1f) / 2) - 1;
        for (int i = 0; i < 4; i++) {
            if (i < selector) {
                drunkBar[i].sprite = drunkennessSpriteMap[2];
            } else if (i > selector) {
                drunkBar[i].sprite = drunkennessSpriteMap[0];
            } else {
                if (crew.drunkenness % 2 == 0) {
                    drunkBar[i].sprite = drunkennessSpriteMap[2];
                } else {
                    drunkBar[i].sprite = drunkennessSpriteMap[1];
                }
            }
        }
    }
}
