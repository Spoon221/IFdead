using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManiacHandText : MonoBehaviour
{
    private ManiacStats maniacStats;
    private ManiacGun maniacGun;

    void Start()
    {
        maniacStats = transform.GetComponentInParent<ManiacStats>();
        maniacGun = transform.GetComponentInParent<ManiacGun>();
        maniacStats.OnManaChanged.AddListener(UpdateVisibility);
    }

    private void UpdateVisibility(float currentMana)
    {
        if (currentMana >= 50 && maniacGun.canShoot)
            gameObject.SetActive(true);
        else
            gameObject.SetActive(false);
    }
}