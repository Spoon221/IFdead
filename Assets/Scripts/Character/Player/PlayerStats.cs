using System;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Events;

public class PlayerStats : Stats
{
    [field: SerializeField] public int MaxHealth { get; private set; }
    [field: SerializeField] public int CurrentHealth { get; private set; }
    [HideInInspector] public UnityEvent<int> OnHealthChanged = new UnityEvent<int>();

    protected override void Start()
    {
        base.Start();
        CurrentHealth = MaxHealth;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Missile missile))
            GetDamage(missile.Damage);
    }

    public void GetDamage(int amountOfDamage)
    {
        CurrentHealth -= amountOfDamage;
        if (CurrentHealth <= 0)
            PhotonNetwork.Disconnect();
        OnHealthChanged.Invoke(CurrentHealth);
    }

    protected void RestoreHealth(int amountOfHealth)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth + amountOfHealth, 0, MaxHealth);
        OnHealthChanged.Invoke(CurrentHealth);
    }
}