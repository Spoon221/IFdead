using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using UnityEngine;
using UnityEngine.UI;

public class Sensivity : MonoBehaviour
{
    void Start()
    {
        gameObject.GetComponent<Slider>().SetValueWithoutNotify(GameSettingSaver.settings.Sensitivity * 100);
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player is not null)
        {
            var playerCamera = player.GetComponentInChildren<ThirdPersonCameraController>();
            playerCamera.SetSensitivitySlider(gameObject.GetComponent<Slider>());
        }
        else
        {
            var maniacCamera = GameObject.FindGameObjectWithTag("Maniac")
                .GetComponentInChildren<FisrtPersonCameraController>();
            maniacCamera.SetSensitivitySlider(gameObject.GetComponent<Slider>());
        }
    }
}