using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Cinemachine;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private ThirdPersonCameraController cameraController;
    //[SerializeField] public AudioMixerSnapshot Normal;
    //[SerializeField] public AudioMixerSnapshot InMenu;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(GameIsPaused)
            {
                Resume();
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Pause();
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }
    
    public void Resume()
    {
        settingsMenu.SetActive(false);
        //Normal.TransitionTo(2f);
        //Time.timeScale = 1f;
        GameIsPaused = false;
        cameraController.cinemachineVirtualCamera.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Pause()
    {
        settingsMenu.SetActive(true);
        //Time.timeScale = 0f;
        GameIsPaused = true;
        cameraController.cinemachineVirtualCamera.enabled = false;
        //InMenu.TransitionTo(2f);
    }
}