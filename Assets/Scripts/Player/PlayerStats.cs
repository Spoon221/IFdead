using System;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Events;

public class PlayerStats : MonoBehaviour
{
    [field: SerializeField] public int MaxHealth { get; private set; }
    private int currentHealth;
    [field: SerializeField] public float MaxMana { get; private set; }
    private float currentMana;
    
    public UnityEvent<int> OnHealthChanged = new UnityEvent<int>();
    public UnityEvent<float> OnManaChanged = new UnityEvent<float>();
    

    private void RestoreHealth(int amountOfHealth)
    {
        currentHealth += amountOfHealth;
        OnHealthChanged.Invoke(currentHealth);
    }

    private void GetDamage(int amountOfDamage)
    {
        currentHealth -= amountOfDamage;
        OnHealthChanged.Invoke(currentHealth);
    }
    
    private void RestoreMana(float amountOfMana)
    {
        currentMana += amountOfMana;
        OnManaChanged.Invoke(currentHealth);
    }
}