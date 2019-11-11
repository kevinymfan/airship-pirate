using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipStatus : MonoBehaviour {
    [SerializeField]
    private Ship ship;

    [SerializeField]
    private Image fuelSlider, airSlider, alcoholSlider;

    void Start() {

    }

    void Update() {
        fuelSlider.fillAmount = ship.GetFuelLevel();
        airSlider.fillAmount = ship.GetAirLevel();
        alcoholSlider.fillAmount = ship.GetAlcoholLevel();
    }
}
