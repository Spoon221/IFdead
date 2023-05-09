using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SmokeCloudSkill : Skill
{
    [SerializeField] private GameObject SmokeVFX;
    [SerializeField] private float timeToDisappear;


    public override void Activate()
    {
        Transform spawnPoint = GameObject.FindGameObjectWithTag("Player").transform;
        GameObject smoke = Instantiate(SmokeVFX, spawnPoint);
        smoke.transform.Rotate(-90f, 0, 0);
        smoke.transform.SetParent(null);
        Destroy(smoke, timeToDisappear);
    }
}