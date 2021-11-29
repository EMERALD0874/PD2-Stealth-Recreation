using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour
{
    [SerializeField] Animator animator;

    bool alert;

    private void Start()
    {
        alert = false;
    }

    private void Update()
    {
        animator.SetBool("IsAlerted", alert);
    }

    public void Alert()
    {
        alert = true;
    }
}