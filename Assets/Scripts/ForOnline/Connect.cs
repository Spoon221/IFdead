using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cinemachine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Connect : MonoBehaviourPunCallbacks
{
    [SerializeField] InputField RoomName;
    [SerializeField] ListItem ItemPrefab;
    [SerializeField] Transform Connecting;

    List<RoomInfo> AllRoomsInfo = new List<RoomInfo>();
    public GameObject Loading;
    public GameObject FindRoom;
    public Canvas lobby;
    public Canvas ESC;
    public static bool GameIsPaused = false;
    public PlayerMovementController scriptPlayerMovementController;
    public ThirdPersonCameraController scriptThirdPersonCameraController;
    [SerializeField] CinemachineVirtualCamera cameraOnTable;
    public Text TextLobbyE;

    [Header("Версия клиента")]
    public string gameVersion = "1"; //Номер версии этого клиента

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion = gameVersion;
        Debug.Log("Версия клиента: " + PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion);
        TextLobbyE.enabled = false;
        cameraOnTable.enabled = false;
        Loading.SetActive(true);
        lobby.enabled = true;
        PhotonNetwork.ConnectUsingSettings();
    }

    private void Awake()
    {
        PhotonNetwork.SendRate = 45; //скорость отправки файлов
        PhotonNetwork.SerializationRate = 45; //скорость принятия файлов
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
        Loading.SetActive(false);
        Debug.Log("Регион подключения: " + PhotonNetwork.CloudRegion);
        PhotonNetwork.JoinLobby();
        lobby.enabled = false;
        TextLobbyE.enabled = true;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (GameIsPaused)
            {
                Resume();
                Cursor.lockState = CursorLockMode.Locked;
                TextLobbyE.enabled = true;
            }
            else
            {
                Pause();
                Cursor.lockState = CursorLockMode.None;
                TextLobbyE.enabled = false;
            }
        }
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Вышел");
    }

    public void Resume()
    {
        cameraOnTable.enabled = false;
        ESC.enabled = true;
        GameIsPaused = false;
        scriptPlayerMovementController.enabled = true;
        scriptThirdPersonCameraController.enabled = true;
    }

    void Pause()
    {
        cameraOnTable.enabled = true;
        ESC.enabled = false;
        GameIsPaused = true;
        scriptPlayerMovementController.enabled = false;
        scriptThirdPersonCameraController.enabled = false;
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