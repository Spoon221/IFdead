using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;

public class ListItem : MonoBehaviour
{
    [SerializeField] Text TextName;
    [SerializeField] Text TextPlayerCount;

    public RoomInfo RoomInfo { get; private set; }

    public void SetInfo(RoomInfo info)
    {
        RoomInfo = info;
        TextName.text = info.Name;
        TextPlayerCount.text = info.PlayerCount + "/" + info.MaxPlayers;
    }

    public void JoinToListRoom()
    {
        PhotonNetwork.JoinRoom(TextName.text);
    }
}
