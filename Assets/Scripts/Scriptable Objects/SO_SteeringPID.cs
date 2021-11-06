using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Steering PID Controller", menuName = "Driving/Steering PID Ctrl")]
public class SO_SteeringPID : ScriptableObject
{
    // Proportional
    public float kP;

    // Integral
    public float kI;

    // Derivative
    public float kD;

}
