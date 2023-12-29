using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using static PlayerHelper;
using System.Linq;

public class SpawnManager : MonoBehaviourPun
{
    public GameObject[] Spawns;
    public GameObject Player;
    public GameObject PlayerLobby;
    public GameObject Maniac;
    [SerializeField] private PlayerReady playerReady;
    private string gameSceneName = "NewGameArea";

    private void Start()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("NextScenePlayer") && SceneManager.GetActiveScene().name == gameSceneName)
        {
            var nextScenePlayer = (string)PhotonNetwork.LocalPlayer.CustomProperties["NextScenePlayer"];
            if (nextScenePlayer == "Player1")
            {
                SpawnManiacOnRoom();
            }
            else if (nextScenePlayer == "Player2")
            {
                SpawnPlayerOnRoom();
            }
        } 
        else 
        {
            SpawnPlayerLobby();
        }
    }

    private void SpawnPlayerOnRoom()
    {
        var randomPosition = GetRandomSpawnPosition();
        var spawnPlayer = PhotonNetwork.Instantiate(Player.name, randomPosition, Quaternion.identity);
        spawnPlayer.GetComponent<PlayerMovementController>().enabled = true;
        RemoveSpawnPoint(randomPosition);
    }

    private void SpawnManiacOnRoom()
    {
        var randomPosition = GetRandomSpawnPosition();
        var spawnManiac = PhotonNetwork.Instantiate(Maniac.name, randomPosition, Quaternion.identity);
        spawnManiac.GetComponent<ManiacMovementController>().enabled = true;
        PhotonNetwork.CurrentRoom.IsVisible = false;
        RemoveSpawnPoint(randomPosition);
    }

    private void SpawnPlayerLobby()
    {
        var spawnPosition = GetPlayerPosition();
        var spawnRotation = GetPlayerRotation();
        if (spawnPosition == Vector3.zero && spawnRotation == Quaternion.identity)
        {
            spawnPosition = GetRandomSpawnPosition();
        }

        var spawnPlayer = PhotonNetwork.Instantiate(PlayerLobby.name, spawnPosition, spawnRotation);
        UpdatePlayerReadyCount();
    }

    private Vector3 GetRandomSpawnPosition()
    {
        var randomIndex = Random.Range(0, Spawns.Length);
        var randomPosition = Spawns[randomIndex].transform.position;
        return randomPosition;
    }

    private void RemoveSpawnPoint(Vector3 spawnPosition)
    {
        for (var i = 0; i < Spawns.Length; i++)
        {
            if (Spawns[i].transform.position == spawnPosition)
            {
                Spawns = Spawns.Where((source, index) => index != i).ToArray();
                break;
            }
        }
    }

    private void UpdatePlayerReadyCount()
    {
        playerReady.countText.text = $"{playerReady.playerReadyStatus.Count}/{PhotonNetwork.PlayerList.Length}";
    }
}