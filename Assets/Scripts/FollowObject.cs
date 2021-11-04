using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    [SerializeField] GameObject objectToFollow;
    [SerializeField] float smoothness;

    Vector3 positionalOffset;
    [SerializeField] Vector3 rotationalOffset;

    void Awake()
    {
        positionalOffset = transform.position;
    }

    void FixedUpdate()
    {
        if (Mathf.Abs(Vector3.Magnitude(transform.position - objectToFollow.transform.position + positionalOffset)) >= 0.1)
            transform.position = Vector3.MoveTowards(transform.position, objectToFollow.transform.position - transform.position + positionalOffset, smoothness * Time.fixedDeltaTime);
    }

    public void SetObjectToFollow(GameObject newObj)
    {
        objectToFollow = newObj;
    }

    //public void RotateCamera()
    //{
    //    return;
    //}
}
