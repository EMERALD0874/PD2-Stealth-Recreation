using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Watcher : MonoBehaviour
{
    [Header("View")]
    [SerializeField] float viewDistance = 7.5f;
    [SerializeField] float viewDistanceMinFalloff = 1f;
    [SerializeField] float viewAngle;
    [SerializeField] LayerMask mask;

    [Header("Detection")]
    [SerializeField] float detectionRate = .5f;
    [SerializeField] float minDetection = .25f;
    [SerializeField] float falloffRate = .2f;

    [Header("Visualization")]
    [SerializeField] GameObject indicator;
    [SerializeField] Sprite suspicious;
    [SerializeField] Sprite detected;

    [Header("Other")]
    [SerializeField] Guard guard;

    SpriteRenderer indicatorRenderer;
    Transform player;
    PlayerDetection detection;
    float sus;
    bool alert;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        detection = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDetection>();
        indicatorRenderer = indicator.GetComponent<SpriteRenderer>();
        sus = 0f;

        alert = false;
        indicatorRenderer.sprite = suspicious;
        indicator.SetActive(false);
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
            {
                detection.NewSuspicion();
                indicator.SetActive(true);
            }
            float susRatio = Mathf.Clamp(Vector3.Distance(transform.position, player.position) - viewDistanceMinFalloff, 0f, viewDistance - viewDistanceMinFalloff) / (viewDistance - viewDistanceMinFalloff);
            susRatio = Mathf.Clamp(-susRatio + 1, minDetection, 1f); // We do this to invert the ratio so closer means quicker
            sus += susRatio * detectionRate * Time.deltaTime;
        }
        else
        {
            sus -= falloffRate * Time.deltaTime;
            if (sus <= 0f)
                indicator.SetActive(false);
        }

        sus = Mathf.Clamp(sus, 0f, 1f);
        detection.UpdateSuspicion(sus);

        if (sus == 1f && !alert)
        {
            alert = true;
            detection.Detected();
            indicator.SetActive(true);
            indicatorRenderer.sprite = detected;

            if (guard != null)
                guard.Alert();

            LevelManager.Instance.GameOver("Alarm tripped: " + guard!=null ? "Police" : "Camera" + "detected suspicious activity");
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

    public void InstantAlert()
    {
        sus = 100f;
    }

    public void Death()
    {
        sus = 0f;
        indicator.SetActive(false);
        Destroy(this);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * viewDistance);
    }
}
