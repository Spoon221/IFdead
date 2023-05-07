using System;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Events;

public class PlayerStats : Stats
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Missile missile))
            GetDamage(missile.Damage);
    }
}