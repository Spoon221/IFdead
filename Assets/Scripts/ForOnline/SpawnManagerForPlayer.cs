using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using System.Linq;

public class SpawnManagerForPlayer : MonoBehaviourPun
{
    public GameObject[] Spawns;
    public GameObject Player;

    [SerializeField] private PlayerReady playerReady;

    //public int PlayerRoom;

    //private void Update()
    //{
    //    var room = PhotonNetwork.CurrentRoom;
    //    PlayerRoom = (int)room.PlayerCount;
    //}

    public void Start()
    {
        SpawnPlayerOnRoom();
        

        /* для тестов
        if (PhotonNetwork.PlayerList.Length == 1)
        {
            var test = PhotonNetwork.Instantiate(Maniac.name, randomPositions, Quaternion.identity);
            test.GetComponent<ManiacMovementController>().enabled = true;

        }
        else if (PhotonNetwork.PlayerList.Length > 1)
        {
            var test1 = PhotonNetwork.Instantiate(Player.name, randomPositions, Quaternion.identity);
            test1.GetComponent<PlayerMovementController>().enabled = true;
        }
        */
    }

    private void SpawnPlayerOnRoom()
    {
        var randomIndex = Random.Range(0, Spawns.Length);
        var randomPosition = Spawns[randomIndex].transform.position;
        var spawnPlayer = PhotonNetwork.Instantiate(Player.name, randomPosition, Quaternion.identity);

        playerReady.countText.text = $"{playerReady.playerReadyStatus.Count}/{PhotonNetwork.PlayerList.Length}";
    }
}