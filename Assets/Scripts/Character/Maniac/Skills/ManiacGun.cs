using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Photon.Pun;

public class ManiacGun : MonoBehaviourPun, IPunObservable
{
    public Transform spawnPoint;
    [SerializeField] private Missile missilePrefab;
    [SerializeField] private PhotonView view;
    private ManiacStats maniacStats;
    private bool canShoot;
    private float missileCooldown;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(spawnPoint.rotation);
        }
        else
        {
            spawnPoint.rotation = (Quaternion)stream.ReceiveNext();
        }
    }

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
                view.RPC("GetHand", RpcTarget.AllBuffered);
            }
        }
    }

    private IEnumerator StartCooldownTimer()
    {
        yield return new WaitForSeconds(missileCooldown);
        canShoot = true;
    }

    [PunRPC]
    void GetHand()
    {
        Instantiate(missilePrefab, spawnPoint.position, spawnPoint.rotation);
        maniacStats.SpendMana(missilePrefab.ManaCost);
        canShoot = false;
        StartCoroutine(StartCooldownTimer());
    }
}