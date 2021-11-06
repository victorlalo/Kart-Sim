using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: BREAK THIS CLASS INTO MULTIPLE MODULAR COMPONENTS
// input
// physics
// visuals
// pathfinding
// AI brain

[System.Serializable]
public class Wheels
{
    // can steer
    public WheelCollider[] frontWheels;

    // can apply motor force
    public WheelCollider[] rearWheels;

    public GameObject[] wheelModels;
}

public class CarController_WC : MonoBehaviour
{
    [SerializeField] SO_CarParams CarParams;
    [SerializeField] float forceMultiplier;

    [SerializeField] Wheels wheels;


    [SerializeField] bool DEBUG_MODE = false;

    Rigidbody rb;
    ParticleSystem particles;

    Vector3 acceleration;
    Vector3 brakingForce;
    Vector3 turnRadius;

    public float currentSpeed = 0f;
    public Vector3 forwardDir; 

    float throttle = 0f;
    float brake = 0f;
    public float steering = 0f;

    public bool isGrounded = false;

    Vector3 startingPos;

    const float BRAKE_LIMIT = 0.5f;
    const float MIN_TURN_SPEED = 0.5f;
    const float DRIFT_LIMIT = 0.75f;

    void Awake()
    {
        //controlSphere.transform.parent = null;
        rb = GetComponent<Rigidbody>();

        particles = GetComponentInChildren<ParticleSystem>();
        particles.enableEmission = false;
        particles.Stop();

        startingPos = transform.position;
    }

    private void Update()
    {
        ////TODO: MOVE THIS TO AWAKE METHOD WHEN TUNED PROPERLY
        //turnRadius = new Vector3(0, CarParams.turnRadius, 0);

        //rb.mass = CarParams.weight;
        //rb.drag = CarParams.straightDrag;
        //rb.angularDrag = CarParams.angularDrag;

        //// MOVE TO AWAKE BETWEEN COMMENTS


        CheckForInput();
        //CheckGroundState();
        //ApplyParticles();




        if (DEBUG_MODE)
        {
            if (Input.GetKeyDown(Inputs.RESET_CAR))
            {
                ResetCar();
            }

            Debug.DrawRay(transform.position, transform.forward.normalized * 5, Color.red);
            Debug.DrawRay(transform.position, rb.velocity, Color.green);
            Debug.DrawRay(transform.position, steering * transform.right.normalized * CarParams.turnRadius, Color.blue);
        }

        ////TODO: Fix wheel animation
        ////AnimateWheels();

    }

    void FixedUpdate()
    {
        //if (!isGrounded)
        //{
        //    return;
        //}

        // Acceleration
        Accelerate();
        Brake();

        // Steering
        Steer();

        // Animation
        //AnimateWheels();
    }

    void CheckForInput()
    {
        throttle = Input.GetKey(Inputs.ACCEL) ? 1f : 0f;
        brake = Input.GetKey(Inputs.BRAKE) ? 1f : 0f;
        steering = Input.GetAxis("Horizontal");
    }

    void CheckGroundState()
    {
        isGrounded = Physics.Raycast(transform.position, -Vector3.up, 0.5f);
    }

    void Accelerate()
    {
        foreach (WheelCollider w in wheels.rearWheels)
        {
            w.motorTorque = throttle * CarParams.acceleration * forceMultiplier;
        }
    }

    void Brake()
    {
        foreach(WheelCollider w in wheels.frontWheels)
        {
            w.brakeTorque = brake * CarParams.brakingForce * forceMultiplier;
        }
        foreach (WheelCollider w in wheels.rearWheels)
        {
            w.brakeTorque = brake * CarParams.brakingForce * forceMultiplier;
        }
    }

    // TODO: Reverse the car

    void Steer()
    {
        foreach (WheelCollider w in wheels.frontWheels)
        {
            w.steerAngle = steering * CarParams.turnRadius;
        }
    }

    void ApplyParticles()
    {
        float driftAmount = Vector3.Dot(transform.forward, rb.velocity.normalized);
        Debug.Log(driftAmount);

        if (steering != 0f && driftAmount < DRIFT_LIMIT && particles.isStopped)
        {
            particles.enableEmission = true;
            particles.Play();
        }
        else if (steering == 0 && driftAmount >= DRIFT_LIMIT && particles.isPlaying)
        {
            particles.enableEmission = false;
            particles.Stop();
        }
    }

    void AnimateWheels()
    {
        Vector3 _pos;
        Quaternion _rot;

        //Front Right
        wheels.frontWheels[0].GetWorldPose(out _pos, out _rot);
        //wheels.wheelModels[0].transform.position = _pos - wheels.frontWheels[0].center;
        wheels.wheelModels[0].transform.rotation = _rot;

        // Front Left
        wheels.frontWheels[1].GetWorldPose(out _pos, out _rot);
        //wheels.wheelModels[1].transform.position = _pos;
        wheels.wheelModels[1].transform.rotation = _rot;

        // Rear Right
        wheels.rearWheels[0].GetWorldPose(out _pos, out _rot);
        //wheels.wheelModels[2].transform.position = _pos;
        wheels.wheelModels[2].transform.rotation = _rot;

        // Rear Left
        wheels.rearWheels[1].GetWorldPose(out _pos, out _rot);
        //wheels.wheelModels[3].transform.position = _pos;
        wheels.wheelModels[3].transform.rotation = _rot;
    }

    public void ResetCar()
    {
        transform.position = startingPos + new Vector3(0, 1, 0);
        //controlSphere.transform.position = transform.position;
        particles.enableEmission = false;
        transform.eulerAngles = Vector3.zero;
        rb.velocity = Vector3.zero;
    }
}
