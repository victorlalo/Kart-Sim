using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public bool isPlayerControlled = true;

    public float throttle = 0f;
    public float brake = 0f;
    public float steering = 0f;

    void Update()
    {
        // If player is providing input, grab key presses
        if (isPlayerControlled)
        {
            throttle = Input.GetAxis("Throttle");
            brake = Input.GetAxis("Brake");
            steering = Input.GetAxis("Horizontal");
        }

        // TODO: Implement AI input handling
        else
        {
            
            return;
        }
    }
}
