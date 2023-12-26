using Photon.Pun;
using System.Collections;
using UnityEngine;

public class AI_Attack : AI_Manager
{
    [SerializeField] private GameObject positionSpawnShot;
    [SerializeField] private Missile prefabShot;

    [SerializeField] private float distanceAttack;

    //[SerializeField] private PhotonView view;

    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    if (stream.IsWriting)
    //    {
    //        stream.SendNext(positionSpawnShot.rotation);
    //    }
    //    else
    //    {
    //        positionSpawnShot.rotation = (Quaternion)stream.ReceiveNext();
    //    }
    //}

    public void CheckingAttackCondition()
    {
        //if (view.IsMine)
        //{
        if (botStatus == BotStatus.chase
            && canShot
            && agent.remainingDistance - agent.stoppingDistance < distanceAttack)
        {
            Shot();
            //view.RPC("Shot", RpcTarget.AllBuffered);
        }
        //}
    }

    [PunRPC]
    public void Shot()
    {
        Instantiate(prefabShot, new Vector3(positionSpawnShot.transform.position.x, 0.5f, positionSpawnShot.transform.position.z), 
            positionSpawnShot.transform.rotation);
        canShot = false;
        StartCoroutine(RechargeGun());
    }

    private IEnumerator RechargeGun()
    {
        yield return new WaitForSeconds(prefabShot.CooldownTime);
        canShot = true;
    }
}
