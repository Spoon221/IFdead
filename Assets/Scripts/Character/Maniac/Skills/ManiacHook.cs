using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ManiacHook : MonoBehaviourPun
{
    public float manaCost;
    [SerializeField] private HookMiss hookMiss;
    [SerializeField] private Transform launchPoint;
    public GameObject missPrefab;


    public HookMiss Miss => hookMiss;

    void Start()
    {
        //maniacStats = GetComponent<ManiacStats>();
        hookMiss = Instantiate(missPrefab).GetComponent<HookMiss>();
        launchPoint = GetComponentInChildren<Camera>().transform;
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetButtonDown("Fire1") && !hookMiss.Hooked)
            {
                photonView.RPC("LaunchHook", RpcTarget.AllBuffered, launchPoint.forward);
                //LaunchHook();
            }
        }
    }

    [PunRPC]
    private void LaunchHook(Vector3 dir)
    {
        //maniacStats.SpendMana(manaCost);
        Miss.Launch(this, dir);
    }
}
