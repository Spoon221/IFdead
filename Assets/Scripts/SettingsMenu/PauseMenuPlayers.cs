using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuPlayers : MonoBehaviour
{
    [SerializeField] private PhotonView view;
    private bool GameIsPaused = false;
    [SerializeField] private ThirdPersonCameraController cameraController;
    [SerializeField] private KeyDownForPlayers.KeyDownForPlayers key;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && view.IsMine)
        {
            if(GameIsPaused)
            {
                GameIsPaused = false;
                key.ResumePlayers();
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                GameIsPaused = true;
                key.PausPlayers();
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }
}