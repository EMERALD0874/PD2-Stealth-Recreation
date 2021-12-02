using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerDetection : MonoBehaviour
{
    [Header("Sounds")]
    [SerializeField] float suspiciousSoundCooldown = 1f;
    [SerializeField] AudioClip suspicious;
    [SerializeField] AudioClip detected;

    [Header("UI")]
    [SerializeField] GameObject detectionPanel;
    [SerializeField] Image leftMeter;
    [SerializeField] Image rightMeter;
    [SerializeField] TextMeshProUGUI exclamationMark;
    [SerializeField] Animator detectedTextAnimator;
    [SerializeField] float startFill;
    [SerializeField] float endFill;

    AudioSource source;
    float mostSuspicious;
    float timeSinceLastSound;
    
    void Start()
    {
        source = GetComponent<AudioSource>();
        mostSuspicious = 0f;
        timeSinceLastSound = suspiciousSoundCooldown;
    }

    void Update()
    {
        timeSinceLastSound += Time.deltaTime;
        if (mostSuspicious == 0f)
            detectionPanel.SetActive(false);
        mostSuspicious = 0f;
    }

    public void NewSuspicion()
    {
        if (timeSinceLastSound >= suspiciousSoundCooldown)
        {
            timeSinceLastSound = 0f;
            source.PlayOneShot(suspicious);
        }
    }

    // UpdateSuspicion should only be called from LateUpdate
    public void UpdateSuspicion(float sus)
    {
        if (sus > mostSuspicious)
        {
            mostSuspicious = sus;
            detectionPanel.SetActive(true);

            float meterFilled = (endFill - startFill) * sus + startFill;

            leftMeter.fillAmount = meterFilled;
            rightMeter.fillAmount = meterFilled;

            int alpha = (int) Mathf.Clamp(((sus * 2) - 1) * 100, 0f, 100f);
            string alphaStr = alpha.ToString("X8");
            exclamationMark.text = "<alpha=#" + alphaStr.Substring(6) + ">!";
        }
    }

    public void Detected()
    {
        source.PlayOneShot(detected);
        detectedTextAnimator.Play("DetectedPrompt");
    }
}
