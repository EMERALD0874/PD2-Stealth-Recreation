using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathManager : MonoBehaviour
{
    [Header("Feedback")]
    [SerializeField] AudioClip deathSound;
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
}
