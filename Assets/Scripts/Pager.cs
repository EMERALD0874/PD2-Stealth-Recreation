using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pager : Interactable
{
    [SerializeField] DeathManager dm;

    public override float TimeNeeded
    {
        get
        {
            return 10f;
        }
    }

    public override void CancelHolding()
    {
        LevelManager.Instance.GameOver("Alarm tripped: Criminal disconnected from the call");
    }

    public override string GetDescription()
    {
        return "Hold [F] to answer the pager";
    }

    public override void Interact()
    {
        
    }

    public override void StartHolding()
    {
        dm.isAnswering = true;
    }
}
