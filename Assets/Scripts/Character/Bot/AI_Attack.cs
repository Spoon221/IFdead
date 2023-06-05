using Photon.Pun;
using System.Collections;
using UnityEngine;

public class AI_Attack : AI_Manager
{
    [SerializeField] private GameObject positionSpawnShot;
    [SerializeField] private Missile prefabShot;

    [SerializeField] private float distanceAttack;

    public void ÑheckingAttackCondition()
    {

        if (botStatus == BotStatus.chase
            && canShot
            && agent.remainingDistance - agent.stoppingDistance < 5f)
        {
            Shot();
        }
    }
    private IEnumerator RechargeGun()
    {
        yield return new WaitForSeconds(prefabShot.CooldownTime);
        canShot = true;
    }

    [PunRPC]
    public void Shot()
    {
        Instantiate(prefabShot, new Vector3(positionSpawnShot.transform.position.x, 0.5f, positionSpawnShot.transform.position.z), 
            positionSpawnShot.transform.rotation);
        canShot = false;
        StartCoroutine(RechargeGun());
    }
}
