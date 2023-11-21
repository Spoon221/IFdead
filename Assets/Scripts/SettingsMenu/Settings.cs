using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using System;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System.Collections;

public class Settings : MonoBehaviourPunCallbacks
{
    public Dropdown resolution;

    public PhotonView view;

    public const string Player1LeftProp = "Player1Left";
    public bool player1Left = false;
    //public SpawnManagerForPlayer PlayerOnRoom;

    //private void Start()
    //{
    //    if (SceneManager.GetActiveScene().name == "FindRoom 2")
    //    {
    //        view = PlayerOnRoom.Player.GetComponent<PhotonView>();
    //    }
    //}

    //private void Update()
    //{
    //    if (SceneManager.GetActiveScene().name == "GameArea")
    //        view = GameObject.FindWithTag("Maniac").GetComponent<PhotonView>();
    //}

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
                //var view = gameObject.AddComponent<PhotonView>();
                //view.ViewID = 99;
                if (nextScenePlayer == "Player1")
                {
                    player1Left = true;
                    var props = new Hashtable();
                    props[Player1LeftProp] = true;
                    PhotonNetwork.LocalPlayer.SetCustomProperties(props);
                    LeaveGame();
                }
                else if (nextScenePlayer == "Player2" && PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey(Player1LeftProp) && (bool)PhotonNetwork.LocalPlayer.CustomProperties[Player1LeftProp])
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
        player1Left = true;
        Hashtable props = new Hashtable();
        props[Player1LeftProp] = true;
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
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