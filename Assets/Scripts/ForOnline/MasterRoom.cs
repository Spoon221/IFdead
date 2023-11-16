using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using Cinemachine;
using System.Collections;

public class MasterRoom : MonoBehaviourPunCallbacks
{
    public Button startButton;
    public Button exitButton;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
        base.OnLeftRoom();
    }

    public void LoadLevel()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            var props = new ExitGames.Client.Photon.Hashtable();
            props.Add("StartMatch", true);
            PhotonNetwork.CurrentRoom.SetCustomProperties(props);
        }
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.ContainsKey("StartMatch"))
        {
            var startMatch = (bool)propertiesThatChanged["StartMatch"];
            if (startMatch)
            {
                PhotonNetwork.LoadLevel("GameArea");
            }
        }
    }
}
