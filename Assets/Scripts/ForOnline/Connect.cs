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
    
    [SerializeField] CinemachineVirtualCamera cameraOnTable;
    [Header("������ �������")]
    public string gameVersion; //����� ������ ����� �������

    private void Start()
    {
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
        PhotonNetwork.SendRate = 45; //�������� �������� ������
        PhotonNetwork.SerializationRate = 45; //�������� �������� ������
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

    // ������� ������ ������
    private void ClearRoomList()
    {
        foreach (Transform child in Connecting)
        {
            Destroy(child.gameObject);
        }

        AllRoomsInfo.Clear();
    }

    // �������� �������� ������ ��� �������
    private void CreateRoomItem(RoomInfo info)
    {
        var item = Instantiate(ItemPrefab, Connecting);
        item.GetComponent<ListItem>().SetInfo(info);
        AllRoomsInfo.Add(info);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("FindRoom 2");
        //StartCoroutine(LoadRoomSceneAsync());
        Debug.Log("������� ������� � ���������: " + PhotonNetwork.CurrentRoom.Name);
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