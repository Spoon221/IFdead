using System;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

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
        if (photonView.IsMine && collision.gameObject.TryGetComponent(out Missile missile))
            GetDamage(missile.Damage);
    }

    public void GetDamage(int amountOfDamage)
    {
        CurrentHealth -= amountOfDamage;
        if (CurrentHealth <= 0)
            PhotonNetwork.LeaveRoom();
        OnHealthChanged.Invoke(CurrentHealth);
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);

        base.OnLeftRoom();
    }

    protected void RestoreHealth(int amountOfHealth)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth + amountOfHealth, 0, MaxHealth);
        OnHealthChanged.Invoke(CurrentHealth);
    }
}