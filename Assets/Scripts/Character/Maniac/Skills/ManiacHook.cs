using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ManiacHook : MonoBehaviourPun
{
    [SerializeField] private HookMiss hookMiss;
    [SerializeField] private Transform launchPoint;

    public float cooldown = 2;
    private float nextUseTime;


    public HookMiss Miss => hookMiss;

    void Start()
    {
        //maniacStats = GetComponent<ManiacStats>();
        hookMiss.parentManiac = this;
        launchPoint = GetComponentInChildren<Camera>().transform;
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            if(Time.time > nextUseTime)
                if (Input.GetButtonDown("Fire1") && !hookMiss.Hooked)
                {
                    photonView.RPC("LaunchHook", RpcTarget.AllBuffered, launchPoint.forward);
                    nextUseTime = Time.time + cooldown;
                    //LaunchHook();
                }

        }
    }

    [PunRPC]
    private void LaunchHook(Vector3 dir)
    {
        //maniacStats.SpendMana(manaCost);
        Miss.Launch(dir);
    }
}
