using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

public class Stats : MonoBehaviourPunCallbacks
{
    [field: SerializeField] public float MaxMana { get; private set; }
    [field: SerializeField] public float ManaRecoveryPerSecond { get; protected set; }
    [field: SerializeField] public float CurrentMana { get; private set; }

    [HideInInspector] public UnityEvent<float> OnManaChanged = new UnityEvent<float>();

    protected virtual void Start()
    {
        CurrentMana = MaxMana;
        InvokeRepeating("RegenerateMana", 0f, 1f);
    }


    public void SpendMana(float amountOfMana)
    {
        CurrentMana -= amountOfMana;
        CurrentMana = Mathf.Clamp(CurrentMana, 0, MaxMana);
        OnManaChanged.Invoke(CurrentMana);
    }

    protected void RestoreMana(float amountOfMana)
    {
        CurrentMana += amountOfMana;
        CurrentMana = Mathf.Clamp(CurrentMana, 0, MaxMana);
        OnManaChanged.Invoke(CurrentMana);
    }

    protected void RegenerateMana()
    {
        RestoreMana(ManaRecoveryPerSecond);
    }
}