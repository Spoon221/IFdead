using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeCloudSkill : MonoBehaviour
{
    [field: SerializeField] public float ManaCost { get; private set; }
    [field: SerializeField] public float CooldownTime { get; private set; }
    [SerializeField] private GameObject SmokeVFX;
    [SerializeField] private float timeToDisappear;
    

    public void SpawnSmoke()
    {
        Transform spawnPoint = GameObject.FindGameObjectWithTag("Player").transform;
        GameObject smoke = Instantiate(SmokeVFX, spawnPoint);
        smoke.transform.Rotate(-90f, 0, 0);
        smoke.transform.SetParent(null);
        Destroy(smoke, timeToDisappear);
    }
}