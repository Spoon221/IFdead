using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnOnGameArea : MonoBehaviourPun, IPunObservable
{
    public GameObject[] Spawns;
    public GameObject Player;
    public GameObject Maniac;

    private bool isManiacSpawned = false;
    public int PlayerRoom;
    private List<int> generatedNumbers = new List<int>();

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(isManiacSpawned);
            stream.SendNext(generatedNumbers);
        }
        else
        {
            isManiacSpawned = (bool)stream.ReceiveNext();
            generatedNumbers = (List<int>)stream.ReceiveNext();
        }
    }

    private void Awake()
    {
        PlayerRoom = FindObjectOfType<SpawnManagerForPlayer>().PlayerRoom;
    }

    void Start()
    {
        SpawnRandomPlayer();
    }

    private void SpawnRandomPlayer()
    {
        var randomNumber = GetUniqueRandomNumber();
        var randomIndex = Random.Range(0, Spawns.Length);
        var randomPosition = Spawns[randomIndex].transform.position;
        if (randomNumber == 1 && !isManiacSpawned)
        {
            var spawnManiac = PhotonNetwork.Instantiate(Maniac.name, randomPosition, Quaternion.identity);
            spawnManiac.GetComponent<ManiacMovementController>().enabled = true;
            isManiacSpawned = true;
            print("Выпало число: " + randomNumber);
        }
        else
        {
            var spawnPlayer = PhotonNetwork.Instantiate(Player.name, randomPosition, Quaternion.identity);
            spawnPlayer.GetComponent<PlayerMovementController>().enabled = true;
            print("Выпало число: " + randomNumber);
        }
        var spawnList = new List<GameObject>(Spawns);
        spawnList.RemoveAt(randomIndex);
        Spawns = spawnList.ToArray();
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
