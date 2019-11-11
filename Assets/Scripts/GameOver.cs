using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour {
    [SerializeField]
    private Image blackScreen;
    private float[] aLevels = { 0.2f, 0.4f, 0.6f, 0.8f, .9f, 1 };
    private int level = 0;

    [SerializeField]
    private float transitionDelay = 1;
    private float nextTransition;

    public string gameOverText;

    void Start() {
        nextTransition = Time.time + transitionDelay;
    }

    void Update() {
        if (level < aLevels.Length && Time.time > nextTransition) {
            Color color = blackScreen.color;
            color.a = aLevels[level];
            blackScreen.color = color;
            level += 1;
            nextTransition = Time.time + transitionDelay;
        }
    }
}
