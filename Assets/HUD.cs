using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] CarController car;

    [SerializeField] Image hudBox;
    [SerializeField] Text speedText;
    [SerializeField] Text throttleText;
    [SerializeField] Text brakeText;
    [SerializeField] Text turnText;

    public bool showHUD = true;

    void Start()
    {
        hudBox.gameObject.SetActive(showHUD);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            showHUD = !showHUD;
            hudBox.gameObject.SetActive(showHUD);
        }
        speedText.text = "Speed: " + (int)car.currentSpeed + " km/hr";
    }
}
