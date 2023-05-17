using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Photon.Pun;

public class ManiacGun : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Missile missilePrefab;
    [SerializeField] private PhotonView view;
    private ManiacStats maniacStats;
    private bool canShoot;
    private float missileCooldown;

    void Start()
    {
        view = GetComponent<PhotonView>();
        maniacStats = GetComponent<ManiacStats>();
        canShoot = true;
        missileCooldown = missilePrefab.CooldownTime;
    }


    void Update()
    {
        if (view.IsMine)
        {
            if (Input.GetButton("Fire1") && canShoot && maniacStats.CurrentMana >= missilePrefab.ManaCost)
            {
                Instantiate(missilePrefab, spawnPoint.position, spawnPoint.rotation);
                maniacStats.SpendMana(missilePrefab.ManaCost);
                canShoot = false;
                StartCoroutine(StartCooldownTimer());
            }
        }
            
    }

    private IEnumerator StartCooldownTimer()
    {
        yield return new WaitForSeconds(missileCooldown);
        canShoot = true;
    }
}