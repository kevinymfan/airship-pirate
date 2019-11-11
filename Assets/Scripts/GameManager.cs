using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    [SerializeField]
    private Ship ship;

    [SerializeField]
    private int score;
    [SerializeField]
    private int distance;

    void Start() {
        DontDestroyOnLoad(this.gameObject);
    }

    void Update() {
        if (ship) {
            score = ship.ticksSurvived;
            distance = ship.distanceTravelled;
        }
    }
}
