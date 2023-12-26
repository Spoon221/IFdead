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
    public Text TextLobbyE;
    public CinemachineVirtualCamera cameraOnTable;
    public PlayerMovementController scriptPlayerMovementController;
    public ThirdPersonCameraController scriptThirdPersonCameraController;
    private Canvas gameTable;
    private Camera cameraPlayer;

    void Start()
    {
        if (view.IsMine)
        {
            cameraOnTable = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
            cameraPlayer = GameObject.Find("CameraPlayer").GetComponent<Camera>();
            gameTable = GameObject.Find("CanvasLobby").GetComponent<Canvas>();
            if (SceneManager.GetActiveScene().name == "FindRoom 2")
            {
                Pause();
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
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        cameraOnTable.enabled = false;
        TextLobbyE.enabled = true;
        GameIsPaused = false;
        scriptPlayerMovementController.enabled = true;
        scriptThirdPersonCameraController.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        
    }

    public void Pause()
    {
        TextLobbyE.enabled = false;
        cameraOnTable.enabled = true;
        GameIsPaused = true;
        scriptPlayerMovementController.enabled = false;
        scriptThirdPersonCameraController.enabled = false;
        Cursor.lockState = CursorLockMode.None;
    }

    public void SubsequentCanvas()
    {
        TextLobbyE.enabled = false;
        GameIsPaused = true;
        scriptPlayerMovementController.enabled = false;
        scriptThirdPersonCameraController.enabled = false;
        Cursor.lockState = CursorLockMode.None;
    }
}
