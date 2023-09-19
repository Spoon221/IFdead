using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cinemachine;

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
    public Text hint;

    string gameVersion = "1"; //Номер версии этого клиента
    private void Start()
    {
        PhotonNetwork.GameVersion = gameVersion;
        Debug.Log(PhotonNetwork.GameVersion);
        hint.enabled = false;
        cameraOnTable.enabled = false;
        Cursor.lockState = CursorLockMode.Locked;
        Loading.SetActive(true);
        lobby.enabled=true;
        PhotonNetwork.ConnectUsingSettings();
        
    }
    private void Awake()
    {
        PhotonNetwork.SendRate = 256; //скорость отправки файлов
        PhotonNetwork.SerializationRate = 256; //скорость принятия файлов
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
        Debug.Log(PhotonNetwork.CloudRegion);
        PhotonNetwork.JoinLobby();
        lobby.enabled = false;
        hint.enabled = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (GameIsPaused)
            {
                Resume();
                Cursor.lockState = CursorLockMode.Locked;
                hint.enabled = true;
            }
            else
            {
                Pause();
                Cursor.lockState = CursorLockMode.None;
                hint.enabled = false;
            }
        }
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

    public void ExitButton()
    {
        Application.Quit();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach(var info in roomList) 
        {
            for (int i = 0; i < AllRoomsInfo.Count; i++)
            {
                if (AllRoomsInfo[i].masterClientId == info.masterClientId)
                    return;
            }
            var Item = Instantiate(ItemPrefab, Connecting);

            if (Item != null)
            {
                Item.SetInfo(info);
                AllRoomsInfo.Add(info);
            }
        }
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("GameArea");
    }

    public void JoinRandom()
    {
        PhotonNetwork.JoinRandomRoom();
    }
}
