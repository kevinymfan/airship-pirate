using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBox : MonoBehaviour {
    [SerializeField]
    private Text mainText;
    [SerializeField]
    private Button buttonA, buttonB, buttonX;
    [SerializeField]
    private Button[] crewButtons;

    public void ShowKeepCrewDialogue(string text) {
        mainText.text = text;
        buttonX.gameObject.SetActive(true);
    }

    public void ShowKeepItemDialogue(string text) {
        mainText.text = text;
        buttonA.gameObject.SetActive(true);
        buttonB.gameObject.SetActive(true);
    }

    public void HideDialogue(string text) {
        mainText.text = text;

        buttonA.gameObject.SetActive(false);
        buttonB.gameObject.SetActive(false);
        buttonX.gameObject.SetActive(false);
    }
}
