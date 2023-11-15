using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using System.Linq;

public class SpawnManagerForPlayer : MonoBehaviour
{
    public GameObject[] Spawns;
    public GameObject Player;
    public GameObject Maniac;

    private bool isManiacSpawned = false;
    public int PlayerRoom;
    private List<int> generatedNumbers = new List<int>();

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "FindRoom 2") 
        {
            var room = PhotonNetwork.CurrentRoom;
            PlayerRoom = (int)room.PlayerCount;
        }
    }

    public void Start()
    {
        if (SceneManager.GetActiveScene().name == "FindRoom 2")
            SpawnPlayerOnRoom();
        else
        {
            CountPlayer();
            SpawnRandomPlayer();
        }
            

        /* ��� ������
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
        PlayerRoom = PlayerPrefs.GetInt("Players");
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
        var randomIndex = Random.Range(0, Spawns.Length);
        var randomPosition = Spawns[randomIndex].transform.position;
        var spawnPlayer = PhotonNetwork.Instantiate(Player.name, randomPosition, Quaternion.identity);
        CountPlayer();
    }

    public int GetUniqueRandomNumber()
    {
        // Создаем список доступных чисел
        List<int> availableNumbers = new List<int>();
        for (int i = 1; i <= PlayerRoom; i++)
            availableNumbers.Add(i);
        // Удаляем уже выбранные числа из списка доступных чисел
        foreach (int selectedNumber in generatedNumbers)
            availableNumbers.Remove(selectedNumber);
        // Если все числа уже выбраны, сбрасываем список выбранных чисел
        if (availableNumbers.Count == 0)
        {
            generatedNumbers.Clear();
            availableNumbers = new List<int>(Enumerable.Range(1, PlayerRoom));
        }
        // Выбираем случайное число из доступных чисел
        int randomNumberIndex = Random.Range(0, availableNumbers.Count);
        int randomNumber = availableNumbers[randomNumberIndex];
        // Добавляем выбранное число в список выбранных чисел
        generatedNumbers.Add(randomNumber);
        return randomNumber;
    }

    public void CountPlayer()
    {
        var room = PhotonNetwork.CurrentRoom;
        PlayerRoom = (int)room.PlayerCount;
        print("Кол-во игроков: " + PlayerRoom);
        PlayerPrefs.SetInt("Players", PlayerRoom);
    }
}