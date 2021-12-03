using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pearl : Interactable
{
    public override float TimeNeeded {
        get
        {
            return 1f;
        }
    }

    public override void CancelHolding()
    {
        
    }

    public override string GetDescription()
    {
        return "Press [F] to steal the pearl";
    }

    public override void Interact()
    {
        LevelManager.Instance.GameWon("The Pearl Job: Success");
        gameObject.SetActive(false);
    }

    public override void StartHolding()
    {
        
    }
}
