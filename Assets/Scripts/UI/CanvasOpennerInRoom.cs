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
    public string cameraOnTableName;
    public PlayerMovementController scriptPlayerMovementController;
    public ThirdPersonCameraController scriptThirdPersonCameraController;

    void Start()
    {
        TextLobbyE.enabled = true;
        cameraOnTable = GameObject.Find(cameraOnTableName).GetComponent<CinemachineVirtualCamera>();
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
