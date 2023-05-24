using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillManager : MonoBehaviourPun
{
    public GameObject smokeCloudSkill;
    private bool isSmokeReady;
    private float smokeManaCost;
    private float cooldownTime;
    private PlayerStats playerStats;
    public PhotonView view;

    void Start()
    {
        isSmokeReady = true;
        smokeManaCost = smokeCloudSkill.GetComponent<SmokeCloudSkill>().ManaCost;
        cooldownTime = smokeCloudSkill.GetComponent<SmokeCloudSkill>().CooldownTime;
        playerStats = gameObject.GetComponent<PlayerStats>();
    }

    void Update()
    {
        if (view.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.E) && isSmokeReady)
            {
                photonView.RPC("GetSmoke", RpcTarget.AllBuffered);
            }
        }
    }

    private void MakeSmokeCloudSkillready()
    {
        isSmokeReady = true;
    }

    [PunRPC]
    void GetSmoke()
    {
        isSmokeReady = false;
        Invoke(nameof(MakeSmokeCloudSkillready), cooldownTime);
        smokeCloudSkill.GetComponent<SmokeCloudSkill>().SpawnSmoke();
        playerStats.SpendMana(smokeManaCost);
    }
}