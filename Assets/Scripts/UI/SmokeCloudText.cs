using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeCloudText : MonoBehaviour
{
    private PlayerStats playerStats;
    private PlayerSkillManager skill;
    
    void Start()
    {
        playerStats = transform.GetComponentInParent<PlayerStats>();
        skill = transform.GetComponentInParent<PlayerSkillManager>();
        playerStats.OnManaChanged.AddListener(UpdateVisibility);
    }

    private void UpdateVisibility(float currentMana)
    {
        if (currentMana >= 50 && skill.isSmokeReady)
            gameObject.SetActive(true);
        else
            gameObject.SetActive(false);
    }
    
}
