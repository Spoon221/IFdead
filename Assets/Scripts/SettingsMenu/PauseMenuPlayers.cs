using UnityEngine;

public class PauseMenuPlayers : MonoBehaviour
{
    public static bool GameIsPaused = false;
    [SerializeField] private ThirdPersonCameraController cameraController;
    [SerializeField] private KeyDownForPlayers.KeyDownForPlayers key;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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