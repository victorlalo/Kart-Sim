using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] SO_CarParams CarParams;
    [SerializeField] float speedMultiplier;
    [SerializeField] GameObject turningPoint;
    //[SerializeField] GameObject controlSphere;
    //[SerializeField] Vector3 sphereOffest = new Vector3(0f, 0.5f, 0f);

    [SerializeField] GameObject frontRightWheel;
    [SerializeField] GameObject frontLeftWheel;
    [SerializeField] GameObject backRightWheel;
    [SerializeField] GameObject backLeftWheel;


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

    void Awake()
    {
        //controlSphere.transform.parent = null;
        rb = GetComponent<Rigidbody>();

        particles = GetComponentInChildren<ParticleSystem>();
        particles.enableEmission = false;
        particles.Stop();

        //rb = controlSphere.GetComponent<Rigidbody>();

        startingPos = transform.position;
    }

    private void Update()
    {
        CheckForInput();
        CheckGroundState();

        //TODO: MOVE THIS TO AWAKE METHOD WHEN TUNED PROPERLY
        turnRadius = new Vector3(0, CarParams.turnRadius, 0);

        rb.mass = CarParams.weight;
        rb.drag = CarParams.straightDrag;
        rb.angularDrag = CarParams.angularDrag;

        // MOVE TO AWAKE BETWEEN COMMENTS

        if (steering != 0 && particles.isStopped)
        {
            particles.enableEmission = true;
            particles.Play();
        }
        else if (steering == 0 && particles.isPlaying)
        {
            particles.enableEmission = false;
            particles.Stop();
        }
        

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

        currentSpeed = rb.velocity.magnitude;
        forwardDir = transform.forward;

        //TODO: Fix wheel animation
        //AnimateWheels();

        //transform.position = controlSphere.transform.position - sphereOffest;
    }

    void FixedUpdate()
    {
        if (!isGrounded)
        {
            return;
        }

        // Acceleration/deceleration force
        if (rb.velocity.magnitude < CarParams.topSpeed)
            rb.AddForce(throttle * CarParams.acceleration * transform.forward * speedMultiplier * Time.fixedDeltaTime, ForceMode.Force);

        if (rb.velocity.z > 0.1f || rb.velocity.x > 0.1f)
            rb.AddForce(brake * CarParams.brakingForce * -transform.forward * speedMultiplier * Time.fixedDeltaTime, ForceMode.Force);

        if (Mathf.Abs(steering) > 0.1f && rb.velocity.magnitude > MIN_TURN_SPEED)
        {
            transform.RotateAround(turningPoint.transform.position, transform.up * steering, CarParams.turnRadius * Time.fixedDeltaTime);
            rb.AddForce(steering * CarParams.turnRadius * transform.right.normalized * speedMultiplier * Time.fixedDeltaTime, ForceMode.Force);
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

    void CheckGroundState()
    {
        isGrounded = Physics.Raycast(turningPoint.transform.position, -Vector3.up, 0.5f);
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

    void AnimateWheels()
    {
        float currSpeed = rb.velocity.magnitude;
        float currTurn = steering * 30f;

        frontRightWheel.transform.Rotate(currSpeed * Time.deltaTime, currTurn * Time.deltaTime, 0);
        frontLeftWheel.transform.Rotate(currSpeed * Time.deltaTime, currTurn * Time.deltaTime, 0);
        backRightWheel.transform.Rotate(currSpeed * Time.deltaTime, 0, 0);
        backLeftWheel.transform.Rotate(currSpeed * Time.deltaTime, 0, 0);
    }

    public void ResetCar()
    {
        transform.position = startingPos + new Vector3(0, 1, 0);
        //controlSphere.transform.position = transform.position;
        transform.eulerAngles = Vector3.zero;
        rb.velocity = Vector3.zero;
        acceleration = transform.forward * CarParams.acceleration;
    }
}
