using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: BREAK THIS CLASS INTO MULTIPLE MODULAR COMPONENTS
// input
// physics
// visuals
// pathfinding
// AI brain

public class CarController : MonoBehaviour
{
    [SerializeField] SO_CarParams CarParams;
    [SerializeField] float forceMultiplier;
    [SerializeField] GameObject turningPoint;

    [SerializeField] GameObject frontRightWheel;
    [SerializeField] GameObject frontLeftWheel;
    [SerializeField] GameObject backRightWheel;
    [SerializeField] GameObject backLeftWheel;


    [SerializeField] bool DEBUG_MODE = false;
    [SerializeField] bool PLAYER_CONTROLLED = true;

    Rigidbody rb;
    ParticleSystem[] particles;

    Vector3 acceleration;
    Vector3 brakingForce;
    Vector3 turnRadius;

    public float currentSpeed = 0f;
    public Vector3 forwardDir; 

    public float throttle = 0f;
    public float brake = 0f;
    public float steering = 0f;

    public bool isGrounded = false;

    Vector3 startingPos;

    const float BRAKE_LIMIT = 0.5f;
    const float MIN_TURN_SPEED = 3f;
    [SerializeField] float DRIFT_LIMIT = 0.95f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        particles = GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem p in particles)
        {
            p.enableEmission = false;
            p.Stop();
        }
        

        startingPos = transform.position;
    }

    private void Update()
    {
        //TODO: MOVE THIS TO AWAKE METHOD WHEN TUNED PROPERLY
        //turnRadius = new Vector3(0, CarParams.turnRadius, 0);

        //rb.mass = CarParams.weight;
        //rb.drag = CarParams.straightDrag;
        //rb.angularDrag = CarParams.angularDrag;

        // MOVE TO AWAKE BETWEEN COMMENTS ^^^

        if (PLAYER_CONTROLLED)
            CheckForInput();

        CheckGroundState();
        ApplyParticles();

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

    }

    void FixedUpdate()
    {
        if (!isGrounded)
        {
            return;
        }

        Accelerate();
        Brake();
        Steer();

        
    }

    void CheckForInput()
    {
        throttle = Input.GetAxis("Throttle");
        brake = Input.GetAxis("Brake");
        steering = Input.GetAxis("Horizontal");
    }

    void CheckGroundState()
    {
        isGrounded = Physics.Raycast(turningPoint.transform.position, -Vector3.up, 0.5f);
    }

    void Accelerate()
    {
        if (rb.velocity.magnitude < CarParams.topSpeed)
            rb.AddForce(throttle * CarParams.acceleration * transform.forward * forceMultiplier * Time.fixedDeltaTime, ForceMode.Force);
    }

    void Brake()
    {
        if (rb.velocity.z > 0.1f || rb.velocity.x > 0.1f)
            rb.AddForce(brake * CarParams.brakingForce * -transform.forward * forceMultiplier * Time.fixedDeltaTime, ForceMode.Force);
    }

    // TODO: Reverse the car

    void Steer()
    {
        if (Mathf.Abs(steering) > 0.1f && rb.velocity.magnitude > MIN_TURN_SPEED)
        {
            transform.RotateAround(turningPoint.transform.position, transform.up * steering, CarParams.turnRadius * Time.fixedDeltaTime);
            rb.AddForce(steering * CarParams.frictionForce * transform.right * forceMultiplier * Time.fixedDeltaTime, ForceMode.Force);
            rb.AddForce(transform.forward * steering, ForceMode.Acceleration);
        }

    }

    void ApplyParticles()
    {
        if (!isGrounded)
        {
            foreach (ParticleSystem p in particles)
            {
                p.enableEmission = false;
                p.Stop();
                p.gameObject.SetActive(false);
            }
        }

        float driftAmount = Vector3.Dot(transform.forward.normalized, rb.velocity.normalized);
        //Debug.Log(driftAmount);

        if (rb.velocity.magnitude > 35f && Mathf.Abs(steering) > 0.1f && Mathf.Abs(driftAmount - 1) < DRIFT_LIMIT && particles[0].isStopped)
        {
            foreach (ParticleSystem p in particles)
            {
                if (!p.gameObject.activeInHierarchy)
                {
                    p.gameObject.SetActive(true);
                }
                p.enableEmission = true;
                p.Play();
            }
                
        }
        else if ((rb.velocity.magnitude < 35f || Mathf.Abs(steering) < 0.1f || Mathf.Abs(driftAmount - 1) >= DRIFT_LIMIT) && particles[0].isPlaying)
        {
            foreach (ParticleSystem p in particles)
            {
                p.enableEmission = false;
                p.Stop();
            }
        }
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

        foreach(ParticleSystem p in particles)
        {
            p.enableEmission = false;
            p.Stop();
        }
        
        transform.eulerAngles = Vector3.zero;
        rb.velocity = Vector3.zero;
        acceleration = transform.forward * CarParams.acceleration;
    }
}
