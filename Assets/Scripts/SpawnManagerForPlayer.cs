using UnityEngine;
using Photon.Pun;
using System.Collections;
using Cinemachine;

public class SpawnManagerForPlayer : MonoBehaviour
{
    public GameObject[] Spawns;
    public GameObject Player;


    public void Start()
    {
        var randomPositions = Spawns[Random.Range(0, Spawns.Length)].transform.localPosition;

        PhotonNetwork.Instantiate(Player.name, randomPositions, Quaternion.identity);
    }
}
