using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;
using static PlayerHelper;

public class Connect : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject player;
    [SerializeField] private Settings settings;
    [SerializeField] private CanvasOpennerInRoom canvasOpennerInRoom;
    [SerializeField] private GameObject playerModel;
    [SerializeField] private TMP_InputField RoomName;
    [SerializeField] private ListItem ItemPrefab;
    [SerializeField] private Transform Connecting;
    public GameObject Loading;
    public Canvas lobby;
    public Text TextLobbyE;
    [SerializeField] private CinemachineVirtualCamera cameraOnTable;
    [Header("Версия клиента")]
    public string gameVersion;
    private int TickRate = 64;
    private List<RoomInfo> AllRoomsInfo = new List<RoomInfo>();

    private void Start()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey(PlayerPositionKey) 
            && PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey(PlayerRotationKey))
        {
            var playerPosition = (Vector3)PhotonNetwork.LocalPlayer.CustomProperties[PlayerPositionKey];
            var playerRotation = (Quaternion)PhotonNetwork.LocalPlayer.CustomProperties[PlayerRotationKey];
            
            if (playerPosition != Vector3.zero && playerRotation != Quaternion.identity)
            {
                SpawnPlayerLobby(player, cameraOnTable, playerModel);
            }
        }
        else
        {
            TextLobbyE.enabled = false;
            Cursor.lockState = CursorLockMode.Locked;
            Loading.SetActive(true);
            lobby.enabled = true;
        }

        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion = gameVersion;
        Debug.Log("Версия клиента: " + PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion);
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
        TextLobbyE.enabled = cameraOnTable.enabled 
            ? false 
            : true;
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
        SavePlayerPosition(player, playerModel);
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