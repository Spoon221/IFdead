using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Cinemachine;
using UnityEngine.SceneManagement;

public class OpeningCanvasRoom : MonoBehaviour
{
    public PhotonView view;
    public bool GameIsPaused = false;
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
                GameIsPaused = true;
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
                Cursor.lockState = CursorLockMode.Locked;
                GameIsPaused = false;
                key.ResumeItermediateScene();
                cameraOnTable.enabled = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                GameIsPaused = true;
                key.PauseItermediateScene();
                cameraOnTable.enabled = true;
            }
        }
    }
}
