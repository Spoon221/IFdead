using UnityEngine;
using Photon.Pun;

public class SpawnManager : MonoBehaviourPun
{
    public GameObject[] Spawns;
    public GameObject Player;
    public GameObject PlayerLobby;
    public GameObject Maniac;
    [SerializeField] private PlayerReady playerReady;

    private void Start()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("NextScenePlayer"))
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
    }

    private void SpawnManiacOnRoom()
    {
        var randomPosition = GetRandomSpawnPosition();
        var spawnManiac = PhotonNetwork.Instantiate(Maniac.name, randomPosition, Quaternion.identity);
        spawnManiac.GetComponent<ManiacMovementController>().enabled = true;
        PhotonNetwork.CurrentRoom.IsVisible = false;
    }

    private void SpawnPlayerLobby()
    {
        var randomPosition = GetRandomSpawnPosition();
        var spawnManiac = PhotonNetwork.Instantiate(PlayerLobby.name, randomPosition, Quaternion.identity);
        UpdatePlayerReadyCount();
    }

    private Vector3 GetRandomSpawnPosition()
    {
        var randomIndex = Random.Range(0, Spawns.Length);
        var randomPosition = Spawns[randomIndex].transform.position;
        return randomPosition;
    }

    private void UpdatePlayerReadyCount()
    {
        playerReady.countText.text = $"{playerReady.playerReadyStatus.Count}/{PhotonNetwork.PlayerList.Length}";
    }
}