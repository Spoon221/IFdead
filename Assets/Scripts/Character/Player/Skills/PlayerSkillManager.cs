using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillManager : MonoBehaviour
{
    public GameObject smokeCloudSkill;
    private bool isSmokeReady;
    private float smokeManaCost;
    private float cooldownTime;
    private PlayerStats playerStats;

    void Start()
    {
        isSmokeReady = true;
        smokeManaCost = smokeCloudSkill.GetComponent<SmokeCloudSkill>().ManaCost;
        cooldownTime = smokeCloudSkill.GetComponent<SmokeCloudSkill>().CooldownTime;
        playerStats = gameObject.GetComponent<PlayerStats>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isSmokeReady)
        {
            isSmokeReady = false;
            Invoke(nameof(MakeSmokeCloudSkillready), cooldownTime);
            smokeCloudSkill.GetComponent<SmokeCloudSkill>().SpawnSmoke();
            playerStats.SpendMana(smokeManaCost);
        }
    }

    private void MakeSmokeCloudSkillready()
    {
        isSmokeReady = true;
    }
}