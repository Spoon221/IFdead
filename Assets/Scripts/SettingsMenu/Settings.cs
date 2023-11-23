using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class Settings : MonoBehaviourPunCallbacks
{
    public Dropdown resolution;

    public PhotonView view;

    public void ChangeResolution()
    {
        if (resolution.value == 0)
        {
            Screen.SetResolution(1366, 768, true);
        }
        if (resolution.value == 1)
        {
            Screen.SetResolution(1920, 1080, true);
        }
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = !Screen.fullScreen;
    }

    public void LeaveRoom()
    {
        if (SceneManager.GetActiveScene().name == "GameArea")
        {
            if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("NextScenePlayer"))
            {
                var nextScenePlayer = (string)PhotonNetwork.LocalPlayer.CustomProperties["NextScenePlayer"];
                if (nextScenePlayer == "Player1")
                {
                    view.RPC("LeaveGame", RpcTarget.All);
                }
                else if (nextScenePlayer == "Player2")
                {
                    LeaveGame();
                    //PhotonNetwork.LeaveRoom();
                }
            }
        }
        else if (SceneManager.GetActiveScene().name == "FindRoom 2")
        {
            PhotonNetwork.LeaveRoom();
        }
    }

    [PunRPC]
    public void LeaveGame()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
        base.OnLeftRoom();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}