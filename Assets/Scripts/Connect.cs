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

    private void Start()
    {
        Loading.SetActive(true);
        FindRoom.SetActive(false);
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
        Debug.Log("ошибка создания лобби");
    }

    public override void OnConnectedToMaster()
    {
        Loading.SetActive(false);
        FindRoom.SetActive(true);
        Debug.Log(PhotonNetwork.CloudRegion);
        PhotonNetwork.JoinLobby();
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
