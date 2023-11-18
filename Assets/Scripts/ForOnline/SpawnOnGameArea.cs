using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnOnGameArea : MonoBehaviourPun
{
    public GameObject[] Spawns;
    public GameObject Player;
    public GameObject Maniac;
    public bool isManiacSpawned = false;
    

    void Start()
    {
        var randomIndex = Random.Range(0, Spawns.Length);
        var randomPosition = Spawns[randomIndex].transform.position;
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("NextScenePlayer"))
        {
            var nextScenePlayer = (string)PhotonNetwork.LocalPlayer.CustomProperties["NextScenePlayer"];
            if (nextScenePlayer == "Player1")
            {
                var spawnManiac = PhotonNetwork.Instantiate(Maniac.name, randomPosition, Quaternion.identity);
                spawnManiac.GetComponent<ManiacMovementController>().enabled = true;
                isManiacSpawned = true;
            }
            else if (nextScenePlayer == "Player2")
            {
                var spawnPlayer = PhotonNetwork.Instantiate(Player.name, randomPosition, Quaternion.identity);
                spawnPlayer.GetComponent<PlayerMovementController>().enabled = true;
            }
        }
    }

    //private void SpawnRandomPlayer()
    //{
    //    var randomNumber = GetUniqueRandomNumber();
    //    var randomIndex = Random.Range(0, Spawns.Length);
    //    var randomPosition = Spawns[randomIndex].transform.position;
    //    if (randomNumber == 1 && !isManiacSpawned)
    //    {
    //        var spawnManiac = PhotonNetwork.Instantiate(Maniac.name, randomPosition, Quaternion.identity);
    //        spawnManiac.GetComponent<ManiacMovementController>().enabled = true;
    //        isManiacSpawned = true;
    //        print("Выпало число: " + randomNumber);
    //    }
    //    else
    //    {
    //        var spawnPlayer = PhotonNetwork.Instantiate(Player.name, randomPosition, Quaternion.identity);
    //        spawnPlayer.GetComponent<PlayerMovementController>().enabled = true;
    //        print("Выпало число: " + randomNumber);
    //    }
    //    var spawnList = new List<GameObject>(Spawns);
    //    spawnList.RemoveAt(randomIndex);
    //    Spawns = spawnList.ToArray();
    //}
}
