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
    private string generatorDisplayTest = "Загрузите чертеж изготовления микросхемы";
    public TextMeshProUGUI text;

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
            generator.GetComponent<Generator>().OnItemActivate.AddListener(UpdateGeneratorText);
    }

    private void UpdateNumberOfKeys()
    {
        currentNumberOfKeys++;
        UpdateText();
    }

    private void UpdateGeneratorText()
    {
        generatorDisplayTest = $"<color=#797373>Загрузка чертежа изготовления микросхемы</color> <color=#F8CE4D>завершена</color>";
        UpdateText();
    }

    private void UpdateText()
    {
        if (currentNumberOfKeys == numberOfKeysToFind)
        {
            var keyText = $"<color=#797373>Соберите ключ:</color> <color=#F8CE4D>{currentNumberOfKeys}/{numberOfKeysToFind}</color>";
            text.text = $"{generatorDisplayTest}\n{keyText}";
        }
        else
        {
            var keyText = $"Соберите ключ: <color=#F8CE4D>{currentNumberOfKeys}/{numberOfKeysToFind}</color>";
            text.text = $"{keyText}\n{generatorDisplayTest}";
        }
    }
}