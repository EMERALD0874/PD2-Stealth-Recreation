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
    [SerializeField] AudioClip[] controlSounds;
    [Header("Other Scripts")]
    [SerializeField] Guard guard;
    [SerializeField] Watcher watcher;
    [Header("Disable on Death")]
    [SerializeField] CapsuleCollider bumpDetection;
    [Header("Enable on Death")]
    [SerializeField] GameObject ragdollParent;
    [SerializeField] Pager pager;

    Collider[] ragdollColliders;
    Rigidbody[] ragdollRbs;

    Animator anim;
    Collider col;

    public bool isAnswering = false;

    private void Start()
    {
        ragdollColliders = ragdollParent.GetComponentsInChildren<Collider>();
        ragdollRbs = ragdollParent.GetComponentsInChildren<Rigidbody>();

        anim = GetComponent<Animator>();
        col = GetComponent<Collider>();

        pager.enabled = false;
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

        AudioSource.PlayClipAtPoint(pagerActiveSound, transform.position, .5f);
        AudioClip control = controlSounds[Random.Range(0, controlSounds.Length)];
        AudioSource.PlayClipAtPoint(control, transform.position, 1f);
        pager.enabled = true;
        yield return new WaitForSeconds(12f);
        if (!isAnswering && !LevelManager.Instance.gameOver)
            LevelManager.Instance.GameOver("Alarm tripped: Pager operator didn't receive an answer");

        yield return null;
    }
}
