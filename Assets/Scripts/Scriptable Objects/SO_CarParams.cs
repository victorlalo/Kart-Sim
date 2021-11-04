using UnityEngine;

[CreateAssetMenu(fileName = "Car Parameters", menuName = "Driving/Car Parameters")]
public class SO_CarParams : ScriptableObject
{
    public string setupName;

    public float acceleration;
    public float topSpeed;
    public float brakingForce;

    public float turnRadius;

    public float weight;
    public float tireWear;


    //public float GetAcceleration()
    //{
    //    return 
    //}
}
