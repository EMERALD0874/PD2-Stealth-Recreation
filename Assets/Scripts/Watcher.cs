using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Watcher : MonoBehaviour
{
    [SerializeField] float viewDistance = 7.5f;
    [SerializeField] float viewDistanceMinFalloff = 1f;
    [SerializeField] float viewAngle;
    [SerializeField] LayerMask mask;
    [SerializeField] float detectionRate = .5f;
    [SerializeField] float minDetection = .25f;
    [SerializeField] float falloffRate = .2f;

    Transform player;
    PlayerDetection detection;
    float sus;
    bool alert;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        detection = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDetection>();
        sus = 0f;

        alert = false;
    }
    
    void LateUpdate()
    {
        if (alert)
        {
            detection.UpdateSuspicion(1f);
            return;
        }

        if (CanSeePlayer())
        {
            if (sus == 0f)
                detection.NewSuspicion();
            float susRatio = Mathf.Clamp(Vector3.Distance(transform.position, player.position) - viewDistanceMinFalloff, 0f, viewDistance - viewDistanceMinFalloff) / (viewDistance - viewDistanceMinFalloff);
            susRatio = Mathf.Clamp(-susRatio + 1, minDetection, 1f); // We do this to invert the ratio so closer means quicker
            sus += susRatio * detectionRate * Time.deltaTime;
        }
        else
        {
            sus -= falloffRate * Time.deltaTime;
        }

        sus = Mathf.Clamp(sus, 0f, 1f);
        detection.UpdateSuspicion(sus);

        if (sus == 1f && !alert)
        {
            alert = true;
            detection.Detected();
        }
    }
    
    bool CanSeePlayer()
    {
        if (Vector3.Distance(transform.position, player.position) < viewDistance)
        {
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, dirToPlayer);
            if (angle < viewAngle / 2f && !Physics.Linecast(transform.position, player.position, mask))
            {
                return true;
            }
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * viewDistance);
    }
}
