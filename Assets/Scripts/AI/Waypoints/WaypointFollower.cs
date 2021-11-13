using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WaypointFollower : MonoBehaviour
{
    WaypointManager wpManager;
    public Vector3 nextWP;
    public int currWaypointIdx;

    Rigidbody rb;
    InputHandler inputHandler;

    public float targetRotation;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        wpManager = WaypointManager._Instance;
        inputHandler = GetComponent<InputHandler>();

        currWaypointIdx = 0;
        nextWP = wpManager.GetWaypoint(currWaypointIdx);
    }

    private void Update()
    {
        CheckDistanceToWaypoint();
        TurnTowardsWaypoint();
        ManageSpeed();
    }

    void CheckDistanceToWaypoint()
    {
        if (Mathf.Abs(Vector3.Distance(transform.position, nextWP)) <= 3f)
        {
            currWaypointIdx = (currWaypointIdx + 1) % wpManager.GetNumWaypoints();
            nextWP = wpManager.GetWaypoint(currWaypointIdx);
        }
    }

    void TurnTowardsWaypoint()
    {
        Vector3 vectorToTarget = nextWP - transform.position;
        vectorToTarget.Normalize();
        targetRotation = Vector3.SignedAngle(transform.forward, vectorToTarget, transform.up)/ 25f;

        inputHandler.steering = targetRotation;

    }

    void ManageSpeed()
    {
        bool isGoingTooFast = Mathf.Abs(targetRotation) > 0.35f;

        // GAS
        inputHandler.throttle = isGoingTooFast? 0 : Mathf.Clamp01(1.05f - Mathf.Abs(targetRotation));

        // BRAKE
        inputHandler.brake = isGoingTooFast ? 1f : 0f;
        
    }

    void FixedUpdate()
    {
        
    }
}
