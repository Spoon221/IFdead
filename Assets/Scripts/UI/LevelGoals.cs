using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelGoals : MonoBehaviour
{
    private List<GameObject> keys;
    private int currentNumberOfKeys;
    private int numberOfKeysToFind;

    private List<GameObject> generators;
    private bool generatorRepaired;
    private string generatorDisplayTest = "Загрузка данных не сделана";

    void Start()
    {
        FindKeys();
        FindGenerators();
        UpdateText();
    }

    private void FindKeys()
    {
        keys = GameObject.FindGameObjectsWithTag("Key").ToList();
        foreach (var key in keys)
            key.GetComponent<PickableItem>().OnItemPickUp.AddListener(UpdateNumberOfKeys);

        currentNumberOfKeys = 0;
        numberOfKeysToFind = keys.Count;
    }

    private void FindGenerators()
    {
        generators = GameObject.FindGameObjectsWithTag("Generator").ToList();
        foreach (var generator in generators)
            generator.GetComponent<ActivatedItem>().OnItemActivate.AddListener(UpdateGeneratorText);
    }

    private void UpdateNumberOfKeys()
    {
        currentNumberOfKeys++;
        UpdateText();
    }

    private void UpdateGeneratorText()
    {
        generatorDisplayTest = "Загрузка данных завершена";
        UpdateText();
    }

    private void UpdateText()
    {
        var keyText = $"ключей найдено: {currentNumberOfKeys}/{numberOfKeysToFind}";
        gameObject.GetComponent<TMP_Text>().text = $"{keyText}\n{generatorDisplayTest}";
    }
}