using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pager : Interactable
{
    [SerializeField] DeathManager dm;
    [SerializeField] AudioClip[] pagerResponses;
    [SerializeField] AudioClip[] controlResponses;
    [SerializeField] GameObject indicator;

    AudioSource src;
    bool answered = false;

    private void Start()
    {
        src = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        indicator.SetActive(true);
    }

    public override float TimeNeeded
    {
        get
        {
            return 10f;
        }
    }

    public override void CancelHolding()
    {
        indicator.SetActive(false);
        if (!answered && !LevelManager.Instance.gameOver)
        {
            src.Stop();
            LevelManager.Instance.GameOver("Alarm tripped: Criminal disconnected from the call");
        }
        enabled = false;
    }

    public override string GetDescription()
    {
        return "Hold [F] to answer the pager";
    }

    public override void Interact()
    {
        AudioClip response = controlResponses[Random.Range(0, controlResponses.Length)];
        src.clip = response;
        src.spatialBlend = 1f;
        src.Play();
        answered = true;
        enabled = false;
    }

    public override void StartHolding()
    {
        AudioClip response = pagerResponses[Random.Range(0, pagerResponses.Length)];
        src.clip = response;
        src.spatialBlend = 0f;
        src.Play();
        dm.isAnswering = true;

        indicator.SetActive(false);
    }
}
