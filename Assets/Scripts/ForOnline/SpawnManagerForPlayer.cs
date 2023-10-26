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
        var randomPosition = Spawns[randomIndex].transform.position;
        var randomNumber = Random.Range(1, 6);

        if (randomNumber == 1 && !isManiacSpawned)
        {
            var spawnManiac = PhotonNetwork.Instantiate(Maniac.name, randomPosition, Quaternion.identity);
            spawnManiac.GetComponent<ManiacMovementController>().enabled = true;
            isManiacSpawned = true;
            print("Число 1");
        }
        else
        {
            var spawnplayer = PhotonNetwork.Instantiate(Player.name, randomPosition, Quaternion.identity);
            spawnplayer.GetComponent<PlayerMovementController>().enabled = true;
            print("Число " + randomNumber);
        }

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
}
