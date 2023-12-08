using System;
using System.Linq;
using Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GeneratorHealthSlider : MonoBehaviour
{
    [SerializeField]private Image slider;
    [SerializeField]private TMP_Text progressText;
    private SingletonGeneratorHealth generatorHealth;

    private void Start()
    {
        gameObject.SetActive(false);
        generatorHealth = SingletonGeneratorHealth.GetInstance();
        //var generators = GameObject.FindGameObjectsWithTag("Generator").ToList();
        //foreach (var generator in generators)
        //{
        //    generator.GetComponent<Generator>().isPlayerInTriggerZone.AddListener(DisplaySlider);
        //    generator.GetComponent<Generator>().isPlayerExitTriggerZone.AddListener(HideSlider);
        //}
    }

    public void DisplaySlider()
    {
        gameObject.SetActive(true);
    }

    private void Update()
    {
        slider.fillAmount = generatorHealth.GetHealth()/1000;
        progressText.text = ((int)(generatorHealth.GetHealth()/10)).ToString();
    }

    public void HideSlider()
    {
        gameObject.SetActive(false);
    }
}