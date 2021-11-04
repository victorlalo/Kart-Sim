using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] SO_CarParams CarParams;
    [SerializeField] float speedMultiplier;
    [SerializeField] GameObject turningPoint;
    [SerializeField] GameObject controlSphere;
    [SerializeField] Vector3 sphereOffest = new Vector3(0f, 0.5f, 0f);

    [SerializeField] bool DEBUG_MODE = false;

    Rigidbody rb;

    Vector3 acceleration;
    Vector3 brakingForce;
    Vector3 turnRadius;

    float throttle = 0f;
    float brake = 0f;
    float steering = 0f;

    Vector3 startingPos;

    const float BRAKE_LIMIT = 0.5f;

    void Awake()
    {
        controlSphere.transform.parent = null;
        rb = controlSphere.GetComponent<Rigidbody>();
        acceleration = transform.forward * CarParams.acceleration;
        brakingForce = transform.forward * CarParams.brakingForce;
        

        startingPos = transform.position;
    }

    private void Update()
    {
        CheckForInput();

        //TODO: MOVE THIS TO AWAKE METHOD WHEN TUNED PROPERLY
        turnRadius = new Vector3(0, CarParams.turnRadius, 0);

        rb.mass = CarParams.weight;
        rb.drag = CarParams.straightDrag;
        rb.angularDrag = CarParams.angularDrag;

        // MOVE TO AWAKE BETWEEN COMMENTS
        

        if (DEBUG_MODE)
        {
            if (Input.GetKeyDown(Inputs.RESET_CAR))
            {
                ResetCar();
            }

            Debug.DrawRay(transform.position, transform.forward.normalized * 5, Color.red);
            Debug.DrawRay(transform.position, rb.velocity, Color.green);
            //Debug.DrawRay
        }

        transform.position = controlSphere.transform.position - sphereOffest;
    }

    void FixedUpdate()
    {
        // Acceleration/deceleration force
        if (rb.velocity.magnitude < CarParams.topSpeed)
            rb.AddForce(throttle * CarParams.acceleration * transform.forward * speedMultiplier * Time.fixedDeltaTime, ForceMode.Force);

        if (rb.velocity.z > 0.1f || rb.velocity.x > 0.1f)
            rb.AddForce(brake * CarParams.brakingForce * -transform.forward * speedMultiplier * Time.fixedDeltaTime, ForceMode.Force);

        if (Mathf.Abs(steering) > 0.1f && rb.velocity.magnitude > 0.1f)
        {
            transform.RotateAround(controlSphere.transform.position, transform.up * steering, CarParams.turnRadius * Time.fixedDeltaTime);
            rb.AddForce(steering * CarParams.turnRadius * transform.right * speedMultiplier * Time.fixedDeltaTime, ForceMode.Force);
            //float error = Vector3.Angle(transform.forward.normalized, rb.velocity.normalized);
            //Debug.Log(error);
            //if (error > 5f)
            //{
            //    rb.velocity += transform.forward * 0.5f;
            //}
            //rb.velocity += rb.velocity.magnitude * transform.forward * Time.fixedDeltaTime;
            //rb.AddForce(CarParams.frictionForce * -transform.forward * Time.fixedDeltaTime, ForceMode.Force);
        }
        
        
    }

    void CheckForInput()
    {
        throttle = Input.GetKey(Inputs.ACCEL) ? 1f : 0f;
        brake = Input.GetKey(Inputs.BRAKE) ? 1f : 0f;

        if (Input.GetKey(Inputs.TURN_RIGHT))
            steering = 1f;

        else if (Input.GetKey(Inputs.TURN_LEFT))
            steering = -1f;
        else
            steering = 0f;

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
        controlSphere.transform.position = transform.position;
        transform.eulerAngles = Vector3.zero;
        rb.velocity = Vector3.zero;
        acceleration = transform.forward * CarParams.acceleration;
    }
}
