using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using System.Collections;

public class Settings : MonoBehaviourPunCallbacks
{
    public Dropdown resolution;

    public PhotonView view;
    private bool LeftGameAllInRoom = false;
    [SerializeField] private GameObject Maniac;
    private Coroutine checkPlayerCoroutine;
    public GameObject LoseCanvas;
    [SerializeField] private ExitForPlayer exit;
    private float displayTime = 5f;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "GameArea")
            LoseCanvas.SetActive(false);
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "GameArea")
        {
            Maniac = GameObject.FindWithTag("Maniac");
            if ((LeftGameAllInRoom || PhotonNetwork.PlayerList.Length == 1 || Maniac == null) && checkPlayerCoroutine == null)
            {
                checkPlayerCoroutine = StartCoroutine(CheckPlayerList());
            }
            else if (!LeftGameAllInRoom && PhotonNetwork.PlayerList.Length > 1 && Maniac != null && checkPlayerCoroutine != null)
            {
                StopCoroutine(checkPlayerCoroutine);
                checkPlayerCoroutine = null;
            }
        }
    }

    private IEnumerator CheckPlayerList()
    {
        yield return new WaitForSeconds(20f); // проверка раз в 20 секунд на кол-во игроков
        StartCoroutine(exit.ShowCanvasAndLeaveGame());
        view.RPC("LeaveGame", RpcTarget.All);
    }

    public IEnumerator ShowLoseCanvas()
    {
        LoseCanvas.SetActive(true);
        yield return new WaitForSeconds(displayTime);
        LoseCanvas.SetActive(false);
    }

    private IEnumerator CurrentLeavegameManiac()
    {
        yield return StartCoroutine(ShowLoseCanvas());
        view.RPC("LeaveGame", RpcTarget.All);
    }

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
                    StartCoroutine(CurrentLeavegameManiac());
                }
                else if (nextScenePlayer == "Player2")
                {
                    StartCoroutine(ShowLoseCanvas());
                    LeaveGame();
                }
            }
            if (PhotonNetwork.PlayerList.Length == 1)
            {
                view.RPC("LeaveGame", RpcTarget.All);
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
        LeftGameAllInRoom = true;
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