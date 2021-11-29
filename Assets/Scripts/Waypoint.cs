using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public bool canStop = true;
    public Waypoint[] connectedWaypoints;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, transform.forward * 2.5f);
        Gizmos.DrawSphere(transform.position, .3f);

        foreach (Waypoint w in connectedWaypoints)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(w.transform.position, .3f);
            Gizmos.DrawLine(transform.position, w.transform.position);
        }
    }
}
