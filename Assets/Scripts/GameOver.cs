using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour {
    [SerializeField]
    private Image blackScreen;
    private float[] aLevels = { 0.2f, 0.4f, 0.6f, 0.8f, .9f, 1 };

    [SerializeField]
    private float transitionDelay = 1;
    private float nextTransition;

    [SerializeField]
    private Text gameOverText, reasonText, timeAliveText, finalScoreText;
    [HideInInspector]
    public string gameOverReason;
    [HideInInspector]
    public int timeAlive;
    [HideInInspector]
    public int finalScore;

    void Start() {
        reasonText.text = gameOverReason;
        timeAliveText.text = "You survived for " + timeAlive + " seconds";
        finalScoreText.text = "Final Score: " + finalScore;
        StartCoroutine(GameOverRoutine());
    }

    private IEnumerator GameOverRoutine() {
        foreach (float a in aLevels) {
            Color color = blackScreen.color;
            color.a = a;
            blackScreen.color = color;
            yield return new WaitForSeconds(1);
        }
        gameOverText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        reasonText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        timeAliveText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        finalScoreText.gameObject.SetActive(true);
    }
}
