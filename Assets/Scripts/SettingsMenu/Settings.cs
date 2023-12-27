using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Realtime;
using static PlayerHelper;

public class Settings : MonoBehaviourPunCallbacks
{
    public GameObject settings;
    public Dropdown ResoDd;

    public PhotonView view;
    private bool LeftGameAllInRoom = false;
    [SerializeField] private GameObject Maniac;
    private Coroutine checkPlayerCoroutine;
    public GameObject LoseCanvas;
    [SerializeField] private ExitForPlayer exit;
    private const float DisplayTime = 5f;

    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Resolution[] resolutions;
    private List<Dropdown.OptionData> odList = new List<Dropdown.OptionData>();

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerModel;
    //private const string PlayerPositionKey = "PlayerPosition";
    //private const string PlayerRotationKey = "PlayerRotation";

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "GameArea")
            LoseCanvas.SetActive(false);
        SetupResolutions();
        fullscreenToggle.SetIsOnWithoutNotify(Screen.fullScreen);
    }

    private void SetupResolutions()
    {

        var resList = new List<Resolution>();
        for (var i = 0; i < Screen.resolutions.Length - 1; i++)
        {
            if(Screen.resolutions[i].height == Screen.resolutions[i+1].height && Screen.resolutions[i].width == Screen.resolutions[i + 1].width) continue;
                resList.Add(Screen.resolutions[i]);
        }
        resList.Add(Screen.resolutions.Last());
        resList.Reverse();
        resolutions = resList.ToArray();

        ResoDd.options.Clear();

        for (var i = 0; i < resolutions.Length; i++)
        {
            odList.Add(new Dropdown.OptionData());

            odList[i].text = ShowResolving(resolutions[i]);

            ResoDd.options.Add(odList[i]);
        }

        ResoDd.onValueChanged.AddListener(index =>
            {
                ResoDd.captionText.text = ShowResolving(resolutions[index]);
                Screen.SetResolution(resolutions[index].width, resolutions[index].height, Screen.fullScreen);
                GameSettingSaver.settings.Resolution = resolutions[index];
                Debug.Log("Resolution now: " + ResoDd.captionText.text + " " + resolutions[index].refreshRate + "Hz");
            });
        ResoDd.captionText.text = ShowResolving(GameSettingSaver.settings.Resolution);

    }

    private static string ShowResolving(Resolution res) => res.width + "X" + res.height;


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
        yield return new WaitForSeconds(20f);
        StartCoroutine(exit.ShowCanvasAndLeaveGame());
        view.RPC("LeaveGame", RpcTarget.All);
    }

    public IEnumerator ShowLoseCanvas()
    {
        LoseCanvas.SetActive(true);
        yield return new WaitForSeconds(DisplayTime);
        LoseCanvas.SetActive(false);
    }

    private IEnumerator CurrentLeavegameManiac()
    {
        yield return StartCoroutine(ShowLoseCanvas());
        view.RPC("LeaveGame", RpcTarget.All);
    }

    //public void ChangeResolution()
    //{
    //    if (resolution.value == 0)
    //    {
    //        Screen.SetResolution(1366, 768, true);
    //    }
    //    if (resolution.value == 1)
    //    {
    //        Screen.SetResolution(1920, 1080, true);
    //    }
    //}

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
            var player = GameObject.FindWithTag("Player");
            var playerModel = GameObject.Find("survivorsModel").gameObject;
            SavePlayerPosition(player, playerModel);
            PhotonNetwork.LeaveRoom();
        }
    }

    [PunRPC]
    public void LeaveGame()
    {
        if (SceneManager.GetActiveScene().name == "FindRoom 2")
        {
            if (photonView.IsMine)
            {
                player = GameObject.FindWithTag("Player");
                //SavePlayerPosition();
            }
        }
        if (SceneManager.GetActiveScene().name == "GameArea")
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

    public void SettingsOpen()
    {
        settings.SetActive(true);
    }

    public void SettingsClose()
    {
        settings.SetActive(false);
    }
}