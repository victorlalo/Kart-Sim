using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    public static WaypointManager _Instance;

    [SerializeField] bool displayWaypoints = false;

    List<GameObject> waypoints = new List<GameObject>();
    GameObject currWP;

    public Vector3 CurrentWaypoint
    {
        get {
            Vector3 wp = currWP.transform.position;
            wp.y = 0;
            return wp; 
        }
    }
    int currWPIdx = 0;

    void Awake()
    {
        // Singleton pattern
        if (_Instance == null)
        {
            _Instance = this;
        }
        else
        {
            Destroy(this);
        }

        foreach(Transform w in transform)
        {
            if (w.gameObject.CompareTag("Waypoint"))
            {
                waypoints.Add(w.gameObject);
                w.gameObject.GetComponent<MeshRenderer>().enabled = displayWaypoints;
            }
        }

        currWP = waypoints[0];
    }

    public Vector3 IncrementWaypoint()
    {
        Vector3 wp = currWP.transform.position;
        wp.y = 0;

        if (currWPIdx >= waypoints.Count)
            currWPIdx = 0;
        currWP = waypoints[currWPIdx++];

        return wp;
    }
}
