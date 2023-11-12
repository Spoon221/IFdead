using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public class SpawnManagerForPlayer : MonoBehaviour
{
    public GameObject[] Spawns;
    public GameObject Player;
    public GameObject Maniac;

    private bool isManiacSpawned = false;
    public int PlayerRoom;
    private List<int> generatedNumbers = new List<int>();

    public void Start()
    {
        if (SceneManager.GetActiveScene().name == "FindRoom 2")
            SpawnPlayerOnRoom();
        else
            SpawnRandomPlayer();

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
            print(randomNumber);
        }
        var spawnList = new List<GameObject>(Spawns);
        spawnList.RemoveAt(randomIndex);
        Spawns = spawnList.ToArray();
    }

    private void SpawnPlayerOnRoom()
    {
        var room = PhotonNetwork.CurrentRoom;
        var randomIndex = Random.Range(0, Spawns.Length);
        var randomPosition = Spawns[randomIndex].transform.position;
        var spawnPlayer = PhotonNetwork.Instantiate(Player.name, randomPosition, Quaternion.identity);
        PlayerRoom = (int)room.PlayerCount;
        print(PlayerRoom);
    }

    public int GetUniqueRandomNumber()
    {
        var randomNumber = Random.Range(1, PlayerRoom + 1);
        while (generatedNumbers.Contains(randomNumber))
        {
            randomNumber = Random.Range(1, PlayerRoom + 1);
        }
        generatedNumbers.Add(randomNumber);
        return randomNumber;
    }
}