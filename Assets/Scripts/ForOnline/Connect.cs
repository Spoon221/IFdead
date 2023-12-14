using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cinemachine;
using UnityEngine.SceneManagement;
using System.Collections;
using ExitGames.Client.Photon;
using TMPro;

public class Connect : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField RoomName;
    [SerializeField] private ListItem ItemPrefab;
    [SerializeField] private Transform Connecting;

    List<RoomInfo> AllRoomsInfo = new List<RoomInfo>();
    public GameObject Loading;
    public GameObject FindRoom;
    public Canvas lobby;
    public Canvas ESC;
    private int TickRate = 64;
    public Text TextLobbyE;
    [SerializeField] CinemachineVirtualCamera cameraOnTable;
    [Header("Версия клиента")]
    public string gameVersion;

    private void Start()
    {
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

                //for (int i = 0; i < 20; i++)
                //{
                //    CreateRoomItem(info);
                //    //tests
                //}
            }
        }
    }

    // Очистка списка комнат
    private void ClearRoomList()
    {
        foreach (Transform child in Connecting)
        {
            Destroy(child.gameObject);
        }

        AllRoomsInfo.Clear();
    }

    // Создание элемента списка для комнаты
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

    private IEnumerator LoadRoomSceneAsync()
    {
        var asyncLoad = SceneManager.LoadSceneAsync("FindRoom 2");
        while (!asyncLoad.isDone)
            yield return null;
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