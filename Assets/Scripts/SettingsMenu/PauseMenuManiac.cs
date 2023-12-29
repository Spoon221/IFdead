using Photon.Pun;
using UnityEngine;

public class PauseMenuManiac : MonoBehaviour
{
    [SerializeField] private PhotonView view;
    private bool GameIsPaused = false;
    [SerializeField] private Camera CameraManiac;
    [SerializeField] private KeyDownForPlayers.KeyDownForPlayers key;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && view.IsMine)
        {
            if (GameIsPaused)
            {
                GameIsPaused = false;
                key.ResumeManiac();
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                GameIsPaused = true;
                key.PausManiac();
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }
}
