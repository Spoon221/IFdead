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
    private int currentNumberOfActivatedGenerators;
    private int numberOfGeneratorsToActivate;


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
            generator.GetComponent<ActivatedItem>().OnItemActivate.AddListener(UpdateNumberOfActivatedGenerators);

        currentNumberOfActivatedGenerators = 0;
        numberOfGeneratorsToActivate = generators.Count;
    }

    private void UpdateNumberOfKeys()
    {
        currentNumberOfKeys++;
        UpdateText();
    }

    private void UpdateNumberOfActivatedGenerators()
    {
        currentNumberOfActivatedGenerators++;
        UpdateText();
    }

    private void UpdateText()
    {
        var keyText = $"Ключей найдено: {currentNumberOfKeys}/{numberOfKeysToFind}";
        var generatorText = $"Генератов включено: {currentNumberOfActivatedGenerators}/{numberOfGeneratorsToActivate}";
        gameObject.GetComponent<TMP_Text>().text = $"Цель: Выбраться со свалки\n{keyText}\n{generatorText}";
    }
}