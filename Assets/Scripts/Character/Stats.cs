using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Stats : MonoBehaviour
{
    [field: SerializeField] public int MaxHealth { get; private set; }

    [field: SerializeField] public float MaxMana { get; private set; }
    [field: SerializeField] public float ManaRecoveryPerSecond { get; protected set; }
    private int currentHealth;
    private float currentMana;

    [HideInInspector] public UnityEvent<int> OnHealthChanged = new UnityEvent<int>();
    [HideInInspector] public UnityEvent<float> OnManaChanged = new UnityEvent<float>();

    private void Start()
    {
        currentHealth = MaxHealth;
        currentMana = MaxMana;
        InvokeRepeating("RegenerateMana", 0f, 1f);
    }


    protected void GetDamage(int amountOfDamage)
    {
        currentHealth -= amountOfDamage;
        if (currentHealth <= 0)
            Destroy(gameObject);
        OnHealthChanged.Invoke(currentHealth);
    }

    protected void RestoreHealth(int amountOfHealth)
    {
        currentHealth = Mathf.Clamp(currentHealth + amountOfHealth, 0, MaxHealth);
        OnHealthChanged.Invoke(currentHealth);
    }

    public void SpendMana(float amountOfMana)
    {
        currentMana -= amountOfMana;
        currentMana = Mathf.Clamp(currentMana, 0, MaxMana);
        OnManaChanged.Invoke(currentMana);
    }


    protected void RestoreMana(float amountOfMana)
    {
        currentMana += amountOfMana;
        currentMana = Mathf.Clamp(currentMana, 0, MaxMana);
        OnManaChanged.Invoke(currentMana);
    }


    protected void RegenerateMana()
    {
        RestoreMana(ManaRecoveryPerSecond);
    }
}