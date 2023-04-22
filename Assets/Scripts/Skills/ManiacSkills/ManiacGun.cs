using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ManiacGun : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;

    [SerializeField] private Missile missilePrefab;
    private bool canShoot;
    private float missileCooldown;

    void Start()
    {
        canShoot = true;
        missileCooldown = missilePrefab.CooldownTime;
    }


    void Update()
    {
        if (Input.GetButton("Fire1") && canShoot)
        {
            Instantiate(missilePrefab, spawnPoint.position, spawnPoint.rotation);
            canShoot = false;
            StartCoroutine(StartCooldownTimer());
        }
    }

    private IEnumerator StartCooldownTimer()
    {
        yield return new WaitForSeconds(missileCooldown);
        canShoot = true;
    }
}