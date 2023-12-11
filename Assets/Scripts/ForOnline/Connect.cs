using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Connect : MonoBehaviourPunCallbacks
{
    [SerializeField] private InputField RoomName;
    [SerializeField] private ListItem ItemPrefab;
    [SerializeField] private Transform Connecting;
    public GameObject Loading;
    public Canvas lobby;
    public Text TextLobbyE;
    [SerializeField] CinemachineVirtualCamera cameraOnTable;
    [Header("������ �������")]
    public string gameVersion;
    private int TickRate = 64;
    private List<RoomInfo> AllRoomsInfo = new List<RoomInfo>();

    private void Start()
    {
        TextLobbyE.enabled = false;
        Cursor.lockState = CursorLockMode.Locked;
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion = gameVersion;
        Debug.Log("������ �������: " + PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion);
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
        Debug.Log("������ �������� �����");
    }

    public override void OnConnectedToMaster()
    {
        TextLobbyE.enabled = true;
        Loading.SetActive(false);
        Debug.Log("������ �����������: " + PhotonNetwork.CloudRegion);
        PhotonNetwork.JoinLobby();
        lobby.enabled = false;
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("�����");
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
        Debug.Log("������� ������� � ���������: " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // �������� ������ �������
        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        // �������� ������ �������
        UpdatePlayerList();
    }

    private void UpdatePlayerList()
    {
        // �������� ������ �������
        foreach (Transform child in Connecting)
        {
            Destroy(child.gameObject);
        }

        // �������� ������ ������� � �������
        Player[] players = PhotonNetwork.PlayerList;

        // ������� �������� ������ ��� ������� ������
        foreach (Player player in players)
        {
            var item = Instantiate(ItemPrefab, Connecting);
            item.GetComponent<ListItem>().SetPlayerInfo(player);
        }
    }

    private IEnumerator LoadRoomSceneAsync()
    {
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
                Debug.Log("������� ������� �������");
            }
            else
            {
                PhotonNetwork.JoinRandomRoom();
                Debug.Log("������������� � ������ �����");
            }
        }
    }
}