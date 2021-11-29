using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] Animator animator;
    [SerializeField] float walkSpeed = 3f;
    [SerializeField] float turnTime = .5f;
    [SerializeField] float waitMin = 3f;
    [SerializeField] float waitMax = 10f;
    [SerializeField] float waitChance = .5f;
    [Header("Waypoints")]
    [SerializeField] Transform waypointParent;
    [SerializeField] Waypoint startingWaypoint;
    [Header("Other Components")]
    [SerializeField] Watcher watcher;

    bool alert;
    GameObject player;

    private void Start()
    {
        alert = false;
        animator.SetBool("IsAlerted", alert);
        player = GameObject.FindGameObjectWithTag("Player");

        StartCoroutine(Patrol(startingWaypoint));
    }

    private void Update()
    {
        if (alert)
        {
            Vector3 playerPos = player.transform.position;
            transform.LookAt(new Vector3(playerPos.x, transform.position.y, playerPos.z));
            transform.rotation *= Quaternion.Euler(0, 45, 0);
        }
    }

    public void Alert()
    {
        alert = true;
        animator.SetBool("IsAlerted", alert);
        watcher.InstantAlert();
    }

    IEnumerator Patrol(Waypoint w)
    {
        Waypoint current = w;
        int i = Random.Range(0, current.connectedWaypoints.Length);
        Waypoint target = current.connectedWaypoints[i];
        transform.LookAt(target.transform);

        animator.SetBool("IsWalking", true);

        while (!alert)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, walkSpeed * Time.deltaTime);

            if (transform.position == target.transform.position)
            {
                // Generate a new waypoint
                current = target;
                i = Random.Range(0, current.connectedWaypoints.Length);
                target = current.connectedWaypoints[i];

                // Decide if we're waiting if we can stop AND chance passes
                // if (current.canStop)
                if (current.canStop && Random.Range(0f, 1f) >= waitChance)
                {
                    animator.SetBool("IsWalking", false);

                    // Rotate to where the current waypoint wants us to look
                    yield return StartCoroutine(Rotate(current.transform.rotation));

                    // Wait a bit
                    float waitTime = Random.Range(waitMin, waitMax);
                    yield return new WaitForSeconds(waitTime);
                }
                
                if (!alert)
                    yield return StartCoroutine(Rotate(Quaternion.LookRotation(target.transform.position - transform.position, Vector3.up)));
                animator.SetBool("IsWalking", true);
            }
            yield return null;
        }
    }

    IEnumerator Rotate(Quaternion q)
    {
        float completion = 0f;
        float startTime = Time.time;
        float turnSpeed = 1 / turnTime;

        Quaternion original = transform.rotation;

        while (completion < 1f)
        {
            completion = (Time.time - startTime) * turnSpeed;
            transform.rotation = Quaternion.Lerp(original, q, completion);
            yield return new WaitForEndOfFrame();
        }
    }

    private void OnDrawGizmosSelected()
    {
        foreach (Transform waypoint in waypointParent)
        {
            Gizmos.DrawSphere(waypoint.position, .3f);
        }
    }
}