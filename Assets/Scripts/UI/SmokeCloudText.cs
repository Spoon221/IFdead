using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeCloudText : MonoBehaviour
{
    private PlayerStats playerStats;
    
    void Start()
    {
        playerStats = transform.GetComponentInParent<PlayerStats>();
        playerStats.OnManaChanged.AddListener(UpdateVisibility);
    }

    private void UpdateVisibility(float currentMana)
    {
        if (currentMana >= 50)
            gameObject.SetActive(true);
        else
            gameObject.SetActive(false);
    }
    
}
