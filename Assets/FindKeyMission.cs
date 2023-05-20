using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FindKeyMission : MonoBehaviour
{
    private List<GameObject> keys;
    private int currentNumberOfKeys;
    private int numberOfKeysToFind;
    private string startWord;

    void Start()
    {
        keys = GameObject.FindGameObjectsWithTag("Key").ToList();
        foreach (var key in keys)
            key.GetComponent<Item>().OnItemPickUp.AddListener(UpdateNumberOfKeys);

        currentNumberOfKeys = 0;
        numberOfKeysToFind = keys.Count;
        startWord = "Цель: Найти ключи\nНайдено ключей";
        gameObject.GetComponent<TMP_Text>().text = $"{startWord}: {currentNumberOfKeys}/{numberOfKeysToFind}";
    }

    private void UpdateNumberOfKeys()
    {
        currentNumberOfKeys += 1;
        gameObject.GetComponent<TMP_Text>().text = $"{startWord}: {currentNumberOfKeys}/{numberOfKeysToFind}";
    }
}