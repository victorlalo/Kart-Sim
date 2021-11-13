using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    // Car Physics Parameters
    [SerializeField] SO_CarParams carParams;
    [SerializeField] float forceMultiplier;
    [SerializeField] float turnMultiplier;
    [SerializeField] float driftFactor;


    // Car States
    float currRotation = 0f;
    Vector3 forwardVelocity;
    public float forwardSpeed;
    Vector3 orthogonalVelocity;
    public float lateralSpeed;

    // Components
    Rigidbody rb;
    InputHandler inputHandler;

    Vector3 initialPos;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        inputHandler = GetComponent<InputHandler>();

        initialPos = transform.position;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetCar();
        }
    }

    void FixedUpdate()
    {
        Accelerate();
        Brake();
        Steer();

        CalculateVelocities();
    }

    void Accelerate()
    {
        if (forwardSpeed < carParams.topSpeed)
            rb.AddForce(transform.forward * carParams.accelerationForce * forceMultiplier * inputHandler.throttle, ForceMode.Force);
    }

    void Brake()
    {
        if (forwardSpeed > 0)
            rb.AddForce(-transform.forward * carParams.brakingForce * forceMultiplier * inputHandler.brake, ForceMode.Force);
    }

    void Steer()
    {
        float minSpeedForTurn = Mathf.Clamp01(rb.velocity.magnitude / 8f);
        //float minSpeedForTurn = 1;

        currRotation += carParams.turnAmount * inputHandler.steering * minSpeedForTurn;
        rb.MoveRotation(Quaternion.Euler(transform.up * currRotation * turnMultiplier * Time.fixedDeltaTime));
    }

    void CalculateVelocities()
    {
        float gravityForce = rb.velocity.y;

        forwardSpeed = Vector3.Dot(rb.velocity, transform.forward);
        forwardVelocity = transform.forward * forwardSpeed;

        lateralSpeed = Vector3.Dot(rb.velocity, transform.right);
        orthogonalVelocity = transform.right * lateralSpeed;

        rb.velocity = forwardVelocity + (orthogonalVelocity * driftFactor);

        // Add in the initial grav force
        Vector3 newVel = rb.velocity;
        newVel.y = gravityForce;
        rb.velocity = newVel;
    }

    public float GetCurrentSpeed()
    {
        return rb.velocity.magnitude;
    }

    void ResetCar()
    {
        transform.position = initialPos + new Vector3(0, 0.5f, 0);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        currRotation = 0f;
    }
}
