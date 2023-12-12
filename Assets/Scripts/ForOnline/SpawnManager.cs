using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public class SpawnManager : MonoBehaviourPun
{
    public GameObject[] Spawns;
    public GameObject Player;
    public GameObject PlayerLobby;
    public GameObject Maniac;
    [SerializeField] private PlayerReady playerReady;

    private const string PlayerPositionKey = "PlayerPosition";
    private const string PlayerRotationKey = "PlayerRotation";

    private void Start()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("NextScenePlayer") && SceneManager.GetActiveScene().name == "GameArea")
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

    private Quaternion GetPlayerRotation()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(PlayerRotationKey, out object rotation))
        {
            return (Quaternion)rotation;
        }

        return Quaternion.Euler(0, 0, 0);
    }

    private Vector3 GetPlayerPosition()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(PlayerPositionKey, out object position))
        {
            return (Vector3)position;
        }

        return Vector3.zero;
    }

    private void SpawnPlayerLobby()
    {
        var spawnPosition = GetPlayerPosition();
        var spawnRotation = GetPlayerRotation();
        if (spawnPosition == Vector3.zero && spawnRotation == Quaternion.identity)
        {
            spawnPosition = GetRandomSpawnPosition();
        }

        var spawnPlayer = PhotonNetwork.Instantiate(PlayerLobby.name, spawnPosition, Quaternion.identity);
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