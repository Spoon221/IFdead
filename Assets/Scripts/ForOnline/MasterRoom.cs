using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using Cinemachine;

public class MasterRoom : MonoBehaviourPunCallbacks
{
    public Button startButton;
    public Button exitButton;


    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
        base.OnLeftRoom();
    }


    [PunRPC]
    public void StartGame()
    {

    }
}
