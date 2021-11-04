using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] SO_CarParams CarParams;
    [SerializeField] float speedMultiplier;
    [SerializeField] GameObject turningPoint;

    [SerializeField] bool DEBUG_MODE = false;

    Rigidbody rb;

    Vector3 acceleration;
    Vector3 brakingForce;
    Vector3 turnRadius;

    Vector3 startingPos;

    const float BRAKE_LIMIT = 0.5f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        acceleration = transform.forward * CarParams.acceleration;
        brakingForce = transform.forward * CarParams.brakingForce;
        turnRadius = new Vector3(0, CarParams.turnRadius, 0);

        startingPos = transform.position;
    }

    private void Update()
    {
        if (DEBUG_MODE)
        {
            if (Input.GetKeyDown(Inputs.RESET_CAR))
            {
                ResetCar();
            }
        }
    }

    void FixedUpdate()
    {
        CheckForInput();
    }

    void CheckForInput()
    {
        if (Input.GetKey(Inputs.ACCEL))
        {
            Accelerate();
        }

        else if (Input.GetKey(Inputs.BRAKE))
        {
            Brake();
        }
        
        if (Input.GetKey(Inputs.TURN_RIGHT))
            Turn(Vector3.up);

        else if (Input.GetKey(Inputs.TURN_LEFT))
            Turn(Vector3.down);

    }

    void Accelerate()
    {
        if (rb.velocity.z < CarParams.topSpeed)
            rb.velocity += acceleration * Time.fixedDeltaTime * speedMultiplier;
    }

    void Brake()
    {
        if (rb.velocity.z >= BRAKE_LIMIT)
        {
            rb.velocity -= brakingForce * Time.fixedDeltaTime * speedMultiplier;
            if (rb.velocity.z == 0)
                rb.velocity = Vector3.zero;
        }
    }

    // TODO: Reverse the car

    void Turn(Vector3 dir)
    {
        if (rb.velocity.magnitude < 0.1f)
        {
            return;
        }
        transform.RotateAround(turningPoint.transform.position, dir, CarParams.turnRadius * Time.fixedDeltaTime);


        //else if (dir == TurnDirection.LEFT)
        //{
        //    //transform.Rotate(0, CarParams.turnRadius * Time.fixedDeltaTime * -1, 0);
        //    transform.RotateAround(turningPoint, Vector3.left, CarParams.turnRadius * Time.fixedDeltaTime);
        //}

        acceleration = transform.forward * CarParams.acceleration;
    }

    public void ResetCar()
    {
        transform.position = startingPos + new Vector3(0, 1, 0);
        transform.eulerAngles = Vector3.zero;
        rb.velocity = Vector3.zero;
        acceleration = transform.forward * CarParams.acceleration;
    }
}
