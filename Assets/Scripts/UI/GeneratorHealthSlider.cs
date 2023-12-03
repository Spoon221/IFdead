using System;
using System.Linq;
using Items;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class GeneratorHealthSlider : MonoBehaviourPunCallbacks
{
    public Slider generatorHealthSlider;
    private Slider slider;
    private SingletonGeneratorHealth generatorHealth;
    private bool isRepairing;
    private PhotonView view;

    private void Start()
    {
        if (photonView.IsMine)
        {
            // view.GetComponent<PhotonView>();
        }

        generatorHealthSlider.gameObject.SetActive(false);
        generatorHealth = SingletonGeneratorHealth.GetInstance();
        var generators = GameObject.FindGameObjectsWithTag("Generator").ToList();
        foreach (var generator in generators)
        {
            generator.GetComponent<Generator>().isPlayerInTriggerZone.AddListener(DisplaySlider);
            generator.GetComponent<Generator>().isPlayerExitTriggerZone.AddListener(HideSlider);
        }
    }

    private void DisplaySlider()
    {
        if (photonView.IsMine)
            generatorHealthSlider.gameObject.SetActive(true);
    }

    private void Update()
    {
        generatorHealthSlider.value = generatorHealth.GetHealth();
    }

    private void HideSlider()
    {
        if (photonView.IsMine)
            generatorHealthSlider.gameObject.SetActive(false);
    }
}