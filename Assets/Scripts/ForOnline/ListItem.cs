using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;
using static PlayerHelper;

public class ListItem : MonoBehaviour
{
    [SerializeField] private Text TextName;
    [SerializeField] private Text TextPlayerCount;
    [SerializeField] private Connect connect;
    [SerializeField] private GameObject player;

    public RoomInfo RoomInfo { get; private set; }

    private void Start()
    {
        connect = FindObjectOfType<Connect>();
        player = connect.gameObject;
    }

    public void SetInfo(RoomInfo info)
    {
        RoomInfo = info;
        TextName.text = info.Name;
        TextPlayerCount.text = info.PlayerCount + "/" + info.MaxPlayers;
    }

    public void JoinToListRoom()
    {
        SavePlayerPosition(player, player); // подумать
        PhotonNetwork.JoinRoom(TextName.text);
    }

    public void SetPlayerInfo(Player player)
    {
        TextName.text = player.NickName;
    }
}
