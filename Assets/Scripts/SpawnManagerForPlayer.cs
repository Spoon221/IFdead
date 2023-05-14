using UnityEngine;
using Photon.Pun;
using System.Collections;
using Cinemachine;

public class SpawnManagerForPlayer : MonoBehaviour
{
    public GameObject[] Spawns;
    public GameObject Player;
    public GameObject Maniac;


    public void Start()
    {
        var randomPositions = Spawns[Random.Range(0, Spawns.Length)].transform.localPosition;
        if (PhotonNetwork.PlayerList.Length == 1)
        {
            PhotonNetwork.Instantiate(Maniac.name, randomPositions, Quaternion.identity);
        }
        else if (PhotonNetwork.PlayerList.Length > 1)
        {
            PhotonNetwork.Instantiate(Player.name, randomPositions, Quaternion.identity);
        }
    }
}
