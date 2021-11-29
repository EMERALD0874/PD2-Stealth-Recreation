using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathManager : MonoBehaviour
{
    [Header("Pager and Feedback")]
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip pagerActiveSound;
    [SerializeField] float timeBeforePagerMin = 1f;
    [SerializeField] float timeBeforePagerMax = 2.5f;
    [Header("Other Scripts")]
    [SerializeField] Guard guard;
    [SerializeField] Watcher watcher;
    [Header("Disable on Death")]
    [SerializeField] CapsuleCollider bumpDetection;
    [Header("Enable on Death")]
    [SerializeField] GameObject ragdollParent;

    Collider[] ragdollColliders;
    Rigidbody[] ragdollRbs;

    Animator anim;
    Collider col;

    private void Start()
    {
        ragdollColliders = ragdollParent.GetComponentsInChildren<Collider>();
        ragdollRbs = ragdollParent.GetComponentsInChildren<Rigidbody>();

        anim = GetComponent<Animator>();
        col = GetComponent<Collider>();

        SetRagdoll(false);
    }

    public void Death()
    {
        guard.Death();
        watcher.Death();

        AudioSource.PlayClipAtPoint(deathSound, transform.position, .5f);
        SetRagdoll(true);

        StartCoroutine(PagerSystem());
    }

    void SetRagdoll(bool enabled)
    {
        anim.enabled = !enabled;

        foreach (Collider c in ragdollColliders)
            c.enabled = enabled;
        foreach (Rigidbody rb in ragdollRbs)
            rb.isKinematic = !enabled;

        col.enabled = !enabled;
    }

    IEnumerator PagerSystem()
    {
        yield return new WaitForSeconds(Random.Range(timeBeforePagerMin, timeBeforePagerMax));

        AudioSource.PlayClipAtPoint(pagerActiveSound, transform.position, 1f);
        Debug.Log("Add pager code here");

        yield return null;
    }
}
