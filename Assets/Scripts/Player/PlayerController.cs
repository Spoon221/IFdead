using System;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    private const float maxSpeed = 4f;
    private const float correctorIateralSpeed = 14f;
    private const float correctorFrontSpeed = 18f;

    [SerializeField] private Transform camera;

    [field: SerializeField] public int MaxHealth { get; private set; }
    private int currentHealth;
    [field: SerializeField] public float MaxMana { get; private set; }
    private float currentMana;
    

    public UnityEvent<int> OnHealthChanged = new UnityEvent<int>();
    public UnityEvent<float> OnManaChanged = new UnityEvent<float>();
    

    private void Awake()
    {
        
    }
    

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
}