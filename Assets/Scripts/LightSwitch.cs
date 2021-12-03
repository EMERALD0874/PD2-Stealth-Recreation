using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : Interactable
{
    [SerializeField] Light m_Light;
    [SerializeField] AudioClip snip;

    bool isOn;

    public override float TimeNeeded
    {
        get
        {
            return 1f;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        isOn = true;
        UpdateLight();
    }

    public override string GetDescription()
    {
        return "Hold [F] to turn the lights off";
    }

    public override void Interact()
    {
        isOn = !isOn;
        UpdateLight();

        AudioSource.PlayClipAtPoint(snip, transform.position, 1f);

        Destroy(this);
    }

    void UpdateLight()
    {
        m_Light.enabled = isOn;
    }

    public override void StartHolding() {}
    public override void CancelHolding() {}
}
