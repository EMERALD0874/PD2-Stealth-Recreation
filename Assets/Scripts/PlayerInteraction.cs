using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] float reach;
    [SerializeField] TextMeshProUGUI prompt;
    [SerializeField] GameObject progressUI;
    [SerializeField] Image progress;

    Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray r = cam.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
        RaycastHit hit;

        bool success = false;

        if (Physics.Raycast(r, out hit, reach))
        {
            Interactable i = hit.collider.GetComponent<Interactable>();

            if (i != null)
            {
                HandleInteraction(i);
                prompt.text = i.GetDescription();
                success = true;

                progressUI.SetActive(i.type == Interactable.InteractionType.Hold && i.GetHoldTime() > 0);
            }
        }

        if (!success)
        {
            prompt.text = "";
            progressUI.SetActive(false);
        }
    }

    void HandleInteraction(Interactable i)
    {
        KeyCode k = KeyCode.F;

        switch (i.type) {
            case Interactable.InteractionType.Instant:
                if (Input.GetKeyDown(k))
                {
                    i.Interact();
                }
                break;
            case Interactable.InteractionType.Hold:
                if (Input.GetKey(k))
                {
                    // If we haven't held down the button at all, say we're starting
                    if (i.GetHoldTime() == 0)
                        i.StartHolding();

                    // Increase time
                    i.IncreaseHoldTime();

                    // If we've held for more time than needed, interact and reset
                    if (i.GetHoldTime() >= i.TimeNeeded)
                    {
                        i.Interact();
                        i.ResetHoldTime();
                    }
                }
                else
                {
                    if (i.GetHoldTime() != 0)
                        i.CancelHolding();
                    i.ResetHoldTime();
                }
                progress.fillAmount = i.GetHoldTime()/i.TimeNeeded;
                break;
            default:
                Debug.Log("Unsupported interactable.");
                break;
        }
    }
}
