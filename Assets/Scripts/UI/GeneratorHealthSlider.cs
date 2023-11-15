using System;
using System.Linq;
using Items;
using UnityEngine;
using UnityEngine.UI;

public class GeneratorHealthSlider : MonoBehaviour
{
    public Slider generatorHealthSlider;
    private Slider slider;
    private SingletonGeneratorHealth generatorHealth;
    private bool isRepairing;

    private void Start()
    {
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
        generatorHealthSlider.gameObject.SetActive(true);
    }

    private void Update()
    {
        
        generatorHealthSlider.value = generatorHealth.GetHealth();
    }

    private void HideSlider()
    {
        generatorHealthSlider.gameObject.SetActive(false);
    }
}