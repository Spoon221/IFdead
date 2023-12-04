using System;
using System.Linq;
using Items;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class GeneratorHealthSlider : MonoBehaviour
{
    public Slider generatorHealthSlider;
    private Slider slider;
    private SingletonGeneratorHealth generatorHealth;
    private bool isRepairing;
    public PhotonView view;

    private void Start()
    {
        generatorHealth = SingletonGeneratorHealth.GetInstance();
        if (view.IsMine)
        {
            generatorHealthSlider.gameObject.SetActive(false);
            var generators = GameObject.FindGameObjectsWithTag("Generator").ToList();
            foreach (var generator in generators)
            {
                generator.GetComponent<Generator>().isPlayerInTriggerZone.AddListener(DisplaySlider);
                generator.GetComponent<Generator>().isPlayerExitTriggerZone.AddListener(HideSlider);
            }
        }
    }

    private void DisplaySlider()
    {
        if (view.IsMine)
            generatorHealthSlider.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (view.IsMine)
            generatorHealthSlider.value = generatorHealth.GetHealth();
    }

    private void HideSlider()
    {
        if (view.IsMine)
            generatorHealthSlider.gameObject.SetActive(false);
    }
}