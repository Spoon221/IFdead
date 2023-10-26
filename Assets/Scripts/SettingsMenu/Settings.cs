using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class Settings : MonoBehaviourPunCallbacks
{
    public Dropdown resolution;
    //public GameObject settingsMenu;

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