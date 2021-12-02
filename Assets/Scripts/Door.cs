using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    [SerializeField] Animator anim;

    public override float TimeNeeded
    {
        get
        {
            return 2f;
        }
    }

    public override void CancelHolding()
    {
        
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
        
    }
}
