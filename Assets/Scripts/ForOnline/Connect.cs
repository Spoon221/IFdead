using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine.SceneManagement;
using System.Collections;
using Unity.VisualScripting;
using ExitGames.Client.Photon;
using TMPro;

public class Connect : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerModel;
    [SerializeField] private TMP_InputField RoomName;
    [SerializeField] private ListItem ItemPrefab;
    [SerializeField] private Transform Connecting;
    public GameObject Loading;
    public Canvas lobby;
    public Text TextLobbyE;
    [SerializeField] CinemachineVirtualCamera cameraOnTable;
    [Header("Версия клиента")]
    public string gameVersion;
    private int TickRate = 64;
    private List<RoomInfo> AllRoomsInfo = new List<RoomInfo>();

    private const string PlayerPositionKey = "PlayerPosition";
    private const string PlayerRotationKey = "PlayerRotation";

    private void Start()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey(PlayerPositionKey) && PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey(PlayerRotationKey))
        {
            var playerPosition = (Vector3)PhotonNetwork.LocalPlayer.CustomProperties[PlayerPositionKey];
            var playerRotation = (Quaternion)PhotonNetwork.LocalPlayer.CustomProperties[PlayerRotationKey];
            
            if (playerPosition != Vector3.zero && playerRotation != Quaternion.identity)
            {
                SpawnPlayerLobby();
            }
        }

        TextLobbyE.enabled = false;
        Cursor.lockState = CursorLockMode.Locked;
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion = gameVersion;
        Debug.Log("Версия клиента: " + PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion);
        cameraOnTable.enabled = false;
        Loading.SetActive(true);
        lobby.enabled = true;
        PhotonNetwork.ConnectUsingSettings();
    }

    private void Awake()
    {
        PhotonNetwork.SendRate = TickRate;
        PhotonNetwork.SerializationRate = TickRate;
    }

    public void CreateRoomButton()
    {
        if (!PhotonNetwork.IsConnected)
        {
            return;
        }
        var room = new RoomOptions();
        room.MaxPlayers = 5;
        PhotonNetwork.CreateRoom(RoomName.text, room, TypedLobby.Default);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("ошибка создания лобби");
    }

    public override void OnConnectedToMaster()
    {
        TextLobbyE.enabled = true;
        Loading.SetActive(false);
        Debug.Log("Регион подключения: " + PhotonNetwork.CloudRegion);
        PhotonNetwork.JoinLobby();
        lobby.enabled = false;

        GetComponent<LobbyBoardController>().SetRegion(PhotonNetwork.CloudRegion);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Вышел");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        ClearRoomList();
        foreach (var info in roomList)
        {
            if (!info.RemovedFromList)
            {
                CreateRoomItem(info);
            }
        }
    }

    private Quaternion GetPlayerRotation()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(PlayerRotationKey, out object rotation) && rotation is Quaternion)
        {
            return (Quaternion)rotation;
        }

        return Quaternion.identity;
    }

    private Vector3 GetPlayerPosition()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(PlayerPositionKey, out object position) && position is Vector3)
        {
            return (Vector3)position;
        }

        return Vector3.zero;
    }

    private void SpawnPlayerLobby()
    {
        var spawnPosition = GetPlayerPosition();
        var spawnRotation = GetPlayerRotation();
        player.transform.position = spawnPosition;
        player.transform.rotation = spawnRotation;
    }

    public void SavePlayerPosition()
    {
        var playerPosition = player.transform.position;
        PhotonNetwork.LocalPlayer.CustomProperties[PlayerPositionKey] = playerPosition;
        var playerRotation = playerModel.transform.rotation;
        PhotonNetwork.LocalPlayer.CustomProperties[PlayerRotationKey] = playerRotation;
    }

    private void ClearRoomList()
    {
        foreach (Transform child in Connecting)
        {
            Destroy(child.gameObject);
        }
        AllRoomsInfo.Clear();
    }

    private void CreateRoomItem(RoomInfo info)
    {
        var item = Instantiate(ItemPrefab, Connecting);
        item.GetComponent<ListItem>().SetInfo(info);
        AllRoomsInfo.Add(info);
    }

    public override void OnJoinedRoom()
    {
        StartCoroutine(LoadRoomSceneAsync());
        Debug.Log("Создана комната с названием: " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerList();
    }

    private void UpdatePlayerList()
    {
        foreach (Transform child in Connecting)
        {
            Destroy(child.gameObject);
        }

        var players = PhotonNetwork.PlayerList;

        foreach (Player player in players)
        {
            var item = Instantiate(ItemPrefab, Connecting);
            item.GetComponent<ListItem>().SetPlayerInfo(player);
        }
    }

    private IEnumerator LoadRoomSceneAsync()
    {
        SavePlayerPosition();
        var asyncLoad = SceneManager.LoadSceneAsync("FindRoom 2");
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        PhotonNetwork.LoadLevel("FindRoom 2");
    }

    public void JoinRandom()
    {
        if (!PhotonNetwork.InRoom)
        {
            if (PhotonNetwork.CountOfRooms == 0)
            {
                CreateRoomButton();
                Debug.Log("Создана обычная комната");
            }
            else
            {
                PhotonNetwork.JoinRandomRoom();
                Debug.Log("Присоединение к рандом лобби");
            }
        }
    }
}