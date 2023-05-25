using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SensivityValue : MonoBehaviour
{
    [SerializeField] private Slider sensitivitySlider;

    void Start()
    {
        ChangeSensitivityValueText(sensitivitySlider.value);
        sensitivitySlider.onValueChanged.AddListener(ChangeSensitivityValueText);
    }

    private void ChangeSensitivityValueText(float sensitivityValue)
    {
        gameObject.GetComponent<Text>().text = Convert.ToInt32(sensitivityValue).ToString();
    }
}