using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    [SerializeField] Animator anim;

    AudioSource src;

    private void Start()
    {
        src = GetComponent<AudioSource>();
    }

    public override float TimeNeeded
    {
        get
        {
            return 3f;
        }
    }

    public override void CancelHolding()
    {
        src.Stop();
    }

    public override string GetDescription()
    {
        return "Hold [F] to pick the lock";
    }

    public override void Interact()
    {
        anim.Play("OpenDoor");
        Destroy(gameObject);
    }

    public override void StartHolding()
    {
        src.Play();
    }
}
