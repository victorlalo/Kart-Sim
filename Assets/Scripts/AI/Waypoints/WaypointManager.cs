using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    public static WaypointManager _Instance;

    [SerializeField] bool displayWaypoints = false;

    List<GameObject> waypoints = new List<GameObject>();

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
            if (!w.gameObject.activeInHierarchy)
            {
                continue;
            }
            if (w.gameObject.CompareTag("Waypoint"))
            {
                waypoints.Add(w.gameObject);
                w.gameObject.GetComponent<MeshRenderer>().enabled = displayWaypoints;
            }
        }
    }

    public int GetNumWaypoints()
    {
        return waypoints.Count;
    }

    public Vector3 GetWaypoint(int idx)
    {
        Vector3 pos = waypoints[idx].transform.position;
        pos.y = 0;
        return pos;
    }
}
