using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Cinemachine;
using UnityEngine.SceneManagement;
using System;

public class OpeningCanvasRoom : MonoBehaviour
{
    [SerializeField] private PhotonView view;
    private static bool GameIsPaused = false;
    public CinemachineVirtualCamera cameraOnTable;
    private Canvas gameTable;
    private Camera cameraPlayer;
    [SerializeField] private KeyDownForPlayers.KeyDownForPlayers key;

    void Start()
    {
        if (view.IsMine)
        {
            cameraOnTable = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
            cameraPlayer = GameObject.Find("CameraPlayer").GetComponent<Camera>();
            gameTable = GameObject.Find("CanvasLobby").GetComponent<Canvas>();
            if (SceneManager.GetActiveScene().name == "FindRoom 2")
            {
                key.PauseItermediateScene();
                cameraOnTable.enabled = true;
            }
            if (gameTable != null)
            {
                gameTable.renderMode = RenderMode.WorldSpace;
                gameTable.worldCamera = cameraPlayer;
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && view.IsMine)
        {
            if (GameIsPaused)
            {
                GameIsPaused = false;
                key.ResumeItermediateScene();
                cameraOnTable.enabled = false;
            }
            else
            {
                GameIsPaused = true;
                key.PauseItermediateScene();
                cameraOnTable.enabled = true;
            }
        }
    }

    public void SubsequentCanvas()
    {
        GameIsPaused = true;
        key.SubsequentCanvasItermediateScene();
    }
}
