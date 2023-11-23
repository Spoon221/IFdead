using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ManiacHook : MonoBehaviourPun
{
    public float manaCost;
    private ManiacStats maniacStats;
    [SerializeField] private HookMiss hookMiss;
    [SerializeField] private Transform launchPoint;

    public HookMiss Miss => hookMiss;

    void Start()
    {
        maniacStats = GetComponent<ManiacStats>();
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetButtonDown("Fire2") && maniacStats.CurrentMana >= manaCost && !hookMiss.Hooked)
            {
                //photonView.RPC("LaunchHook", RpcTarget.AllBuffered);
                LaunchHook();
            }
        }
    }

    [PunRPC]
    private void LaunchHook()
    {
        maniacStats.SpendMana(manaCost);
        Miss.Launch(this, launchPoint.forward);
    }
}
