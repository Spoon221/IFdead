using System;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Events;

public class PlayerStats : MonoBehaviour
{
    [field: SerializeField] public int MaxHealth { get; private set; }

    [field: SerializeField] public float MaxMana { get; private set; }
    private int currentHealth;
    private float currentMana;

    [HideInInspector] public UnityEvent<int> OnHealthChanged = new UnityEvent<int>();
    [HideInInspector] public UnityEvent<float> OnManaChanged = new UnityEvent<float>();

    private void Start()
    {
        currentHealth = MaxHealth;
        currentMana = MaxMana;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Missile missile))
            GetDamage(missile.Damage);
    }

    private void GetDamage(int amountOfDamage)
    {
        currentHealth -= amountOfDamage;
        if (currentHealth <= 0)
            Destroy(gameObject);
        OnHealthChanged.Invoke(currentHealth);
    }

    private void RestoreHealth(int amountOfHealth)
    {
        currentHealth += amountOfHealth;
        OnHealthChanged.Invoke(currentHealth);
    }


    private void RestoreMana(float amountOfMana)
    {
        currentMana += amountOfMana;
        OnManaChanged.Invoke(currentHealth);
    }
}