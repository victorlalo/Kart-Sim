using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Persona - skill level, stats, driving style, motivation for decisions(more on this below)
// Strategy - broad speed and steering goals based on the short-term track information(what’s immediately in front of you)
// Tactics - processed every frame.refines the strategy into concrete numbers.The “fine tuning” phase
// Control - Getting fed the previous Strategy and Tactics information, the AI will now apply values to the three inputs: steer, throttle, and brake

public class CarBrain : MonoBehaviour
{
    [SerializeField] float visionDistance = 10f;
    public bool carInFront = false;

    WaypointManager wpManager;
    Vector3 nextWP;
    int currWaypointIdx;
    float targetRotation;

    InputHandler inputHandler;

    void Start()
    {
        wpManager = WaypointManager._Instance;
        inputHandler = GetComponent<InputHandler>();

        currWaypointIdx = 0;
        nextWP = wpManager.GetWaypoint(currWaypointIdx);
    }

    
    void Update()
    {
        CheckDistanceToWaypoint();
        IsObstacleInFront();
        TurnTowardsTarget();

        ManageSpeed();
    }

    void ManageSpeed()
    {
        bool isGoingTooFast = Mathf.Abs(targetRotation) > 0.35f;

        // GAS
        inputHandler.throttle = isGoingTooFast ? 0 : Mathf.Clamp01(1.05f - Mathf.Abs(targetRotation));

        // BRAKE
        inputHandler.brake = isGoingTooFast ? 1f : 0f;

    }

    bool IsObstacleInFront()
    {
        RaycastHit hit;
        Physics.SphereCast(transform.position, 1f, transform.forward, out hit, visionDistance, 1 << LayerMask.NameToLayer("Car"));

        if (hit.collider != null)
        {
            Vector3 obstaclePos = hit.collider.transform.position;
            Vector3 obstacleRightVector = hit.collider.transform.right;

            Vector3 avoidanceDir = Vector3.Reflect((obstaclePos - transform.position).normalized, obstacleRightVector);
            avoidanceDir.Normalize();

            inputHandler.steering = 0.5f;

        }



        // debug shit
        Color rayColor;
        if (hit.collider != null)
            rayColor = Color.green;
        else
            rayColor = Color.red;

        Debug.DrawLine(transform.position, transform.position + transform.forward * visionDistance, rayColor);

        return hit.collider != null;
    }

    void CheckDistanceToWaypoint()
    {
        if (Mathf.Abs(Vector3.Distance(transform.position, nextWP)) <= 3f)
        {
            currWaypointIdx = (currWaypointIdx + 1) % wpManager.GetNumWaypoints();
            nextWP = wpManager.GetWaypoint(currWaypointIdx);
        }
    }

    void TurnTowardsTarget()
    {
        Vector3 vectorToTarget = nextWP - transform.position;
        vectorToTarget.Normalize();
        targetRotation = Vector3.SignedAngle(transform.forward, vectorToTarget, transform.up) / 30f;

        inputHandler.steering = targetRotation;

    }

}
