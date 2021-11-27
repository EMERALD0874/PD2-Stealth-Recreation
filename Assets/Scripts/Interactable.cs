using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public enum InteractionType
    {
        Instant,
        Hold
    }

    float holdTime;
    public abstract float TimeNeeded { get; }

    public InteractionType type;

    public abstract string GetDescription();
    public abstract void Interact();

    public abstract void StartHolding();
    public abstract void CancelHolding();

    public void IncreaseHoldTime() => holdTime += Time.deltaTime;
    public void ResetHoldTime() => holdTime = 0f;

    public float GetHoldTime() => holdTime;
}
