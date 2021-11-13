using UnityEngine;

[CreateAssetMenu(fileName = "Car Parameters", menuName = "Driving/Car Parameters")]
public class SO_CarParams : ScriptableObject
{
    public string setupName;

    public float topSpeed;
    public float accelerationForce;
    public float brakingForce;
    public float turnAmount;

    public float weight;
    public float tireWear;

}
