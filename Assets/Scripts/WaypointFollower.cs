using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WaypointFollower : MonoBehaviour
{
    [SerializeField] float speedMultiplier = 100f;
    [SerializeField] float offsetRot;

    WaypointManager wpManager;
    Vector3 nextWP;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        wpManager = WaypointManager._Instance;

        nextWP = wpManager.CurrentWaypoint;
    }

    private void Update()
    {
        if (Mathf.Abs(Vector3.Distance(transform.position, nextWP)) <= 3f)
        {
            nextWP = wpManager.IncrementWaypoint();
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(Vector3.MoveTowards(transform.position, nextWP, Time.fixedDeltaTime * speedMultiplier));
        //rb.DOMove(nextWP,)
    }

    //IEnumerator rotateTowards(float angle)
    //{
    //    while (Mathf.Abs(transform.eulerAngles.y - angle) > 0.5f)
    //    {
    //        yield return new WaitForEndOfFrame();

            
    //    }
    //}
}
