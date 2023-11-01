using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Cinemachine;

public class CanvasOpennerInRoom : MonoBehaviour
{
    [SerializeField] PhotonView view;
    public static bool GameIsPaused = false;
    public Text TextLobbyE;
    [SerializeField] CinemachineVirtualCamera cameraOnTable;
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
            gameTable = GameObject.Find("GameTable").GetComponent<Canvas>();
            if (gameTable != null)
            {
                gameTable.renderMode = RenderMode.WorldSpace;
                gameTable.worldCamera = cameraPlayer;
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (GameIsPaused)
            {
                Resume();
                Cursor.lockState = CursorLockMode.Locked;
                TextLobbyE.enabled = true;
                
            }
            else
            {
                Pause();
                Cursor.lockState = CursorLockMode.None;
                TextLobbyE.enabled = false;
            }
        }
    }

    public void Resume()
    {
        cameraOnTable.enabled = false;
        GameIsPaused = false;
        scriptPlayerMovementController.enabled = true;
        scriptThirdPersonCameraController.enabled = true;
    }

    void Pause()
    {
        cameraOnTable.enabled = true;
        GameIsPaused = true;
        scriptPlayerMovementController.enabled = false;
        scriptThirdPersonCameraController.enabled = false;
    }
}
