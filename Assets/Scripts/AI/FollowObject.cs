using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    [SerializeField] GameObject target;
    CarController car;

    public float angleToTarget;
    public float rotationSpeed = 0.3f;

    void Awake()
    {
        car = GameObject.FindGameObjectWithTag("Player").GetComponent<CarController>();
    }

    void Update()
    {
        var targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);
        angleToTarget = targetRotation.eulerAngles.y;

        // Smoothly rotate towards the target point.
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    public void SetObjectToFollow(GameObject newObj)
    {
        target = newObj;
    }

}
