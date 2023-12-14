using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;

public class ListItem : MonoBehaviour
{
    [SerializeField] private Text TextName;
    [SerializeField] private Text TextPlayerCount;
    [SerializeField] private Connect connect;

    public RoomInfo RoomInfo { get; private set; }

    private void Start()
    {
        connect = FindObjectOfType<Connect>();
    }

    public void SetInfo(RoomInfo info)
    {
        RoomInfo = info;
        TextName.text = info.Name;
        TextPlayerCount.text = info.PlayerCount + "/" + info.MaxPlayers;
    }

    public void JoinToListRoom()
    {
        connect.SavePlayerPosition();
        PhotonNetwork.JoinRoom(TextName.text);
    }

    public void SetPlayerInfo(Player player)
    {
        TextName.text = player.NickName;
    }
}
