using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

public class Stats : MonoBehaviour
{
    [field: SerializeField] public int MaxHealth { get; private set; }

    [field: SerializeField] public float MaxMana { get; private set; }
    [field: SerializeField] public float ManaRecoveryPerSecond { get; protected set; }
    [field: SerializeField] public int CurrentHealth { get; private set; }
    [field: SerializeField] public float CurrentMana { get; private set; }

    [HideInInspector] public UnityEvent<int> OnHealthChanged = new UnityEvent<int>();
    [HideInInspector] public UnityEvent<float> OnManaChanged = new UnityEvent<float>();

    private void Start()
    {
        CurrentHealth = MaxHealth;
        CurrentMana = MaxMana;
        InvokeRepeating("RegenerateMana", 0f, 1f);
    }


    protected void GetDamage(int amountOfDamage)
    {
        CurrentHealth -= amountOfDamage;
        if (CurrentHealth <= 0)
            PhotonNetwork.Destroy(gameObject);
        OnHealthChanged.Invoke(CurrentHealth);
    }

    protected void RestoreHealth(int amountOfHealth)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth + amountOfHealth, 0, MaxHealth);
        OnHealthChanged.Invoke(CurrentHealth);
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