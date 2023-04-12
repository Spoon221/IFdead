using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    [SerializeField] public GameObject settingsMenu;
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
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Pause()
    {
        settingsMenu.SetActive(true);
        //Time.timeScale = 0f;
        GameIsPaused = true;
        //InMenu.TransitionTo(2f);
    }
}