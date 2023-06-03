using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

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
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Loading.SetActive(true);
        FindRoom.SetActive(false);
        lobby.enabled=true;
        PhotonNetwork.ConnectUsingSettings();
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
        Debug.Log("������ �������� �����");
    }

    public override void OnConnectedToMaster()
    {
        Loading.SetActive(false);
        Debug.Log(PhotonNetwork.CloudRegion);
        PhotonNetwork.JoinLobby();
        lobby.enabled = false;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (GameIsPaused)
            {
                Resume();
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Pause();
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }
    public void Resume()
    {
        FindRoom.SetActive(false);
        ESC.enabled = true;
        GameIsPaused = false;
    }

    void Pause()
    {
        FindRoom.SetActive(true);
        ESC.enabled = false;
        //Time.timeScale = 0f;
        GameIsPaused = true;
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
        PhotonNetwork.JoinRandomOrCreateRoom();
    }
}
