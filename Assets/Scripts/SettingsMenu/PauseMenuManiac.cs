using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuManiac : MonoBehaviour
{
    public static bool GameIsPaused = false;
    [SerializeField] private Camera CameraManiac;
    [SerializeField] private KeyDownForPlayers.KeyDownForPlayers key;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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
