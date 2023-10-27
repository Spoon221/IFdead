using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;

public class SpawnManagerForPlayer : MonoBehaviour
{
    public GameObject[] Spawns;
    public GameObject Player;
    public GameObject Maniac;

    private bool isManiacSpawned = false;
    public void Start()
    {
        var randomIndex = Random.Range(0, Spawns.Length);

        SpawnRandomPlayer();

        var spawnList = new List<GameObject>(Spawns);
        spawnList.RemoveAt(randomIndex);
        Spawns = spawnList.ToArray();

        /* Для тестов
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

    private void SpawnRandomPlayer()
    {
        var randomIndex = Random.Range(0, Spawns.Length);
        var randomPosition = Spawns[randomIndex].transform.position;

        if (PhotonNetwork.PlayerList.Length < 5)
        {
            var randomNumber = GetUniqueRandomNumber();

            if (randomNumber == 1 && !isManiacSpawned)
            {
                var spawnManiac = PhotonNetwork.Instantiate(Maniac.name, randomPosition, Quaternion.identity);
                spawnManiac.GetComponent<ManiacMovementController>().enabled = true;
                isManiacSpawned = true;
                print(randomNumber);
            }
            else
            {
                var spawnPlayer = PhotonNetwork.Instantiate(Player.name, randomPosition, Quaternion.identity);
                spawnPlayer.GetComponent<PlayerMovementController>().enabled = true;
                Debug.Log("Player spawned");
                print(randomNumber);
            }
        }
    }

    private int GetUniqueRandomNumber()
    {
        var randomNumber = Random.Range(1, 5);

        while (randomNumber == 1 && isManiacSpawned)
        {
            randomNumber = Random.Range(1, 5);
        }

        return randomNumber;
    }
}