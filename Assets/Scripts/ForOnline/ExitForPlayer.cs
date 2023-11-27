using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ExitForPlayer : MonoBehaviour
{
    [SerializeField] private Settings settings;
    [SerializeField] private PhotonView view;

    private void OnTriggerEnter(Collider other)
    {
        var countPlayers = GameObject.FindGameObjectsWithTag("Player").Length;
        if (other.CompareTag("Player"))
        {
            settings.LeaveGame();
            if (countPlayers < 1)
                settings.view.RPC("LeaveGame", RpcTarget.All);
        }
    }
}
