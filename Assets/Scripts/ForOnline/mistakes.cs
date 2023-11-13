using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mistakes : MonoBehaviour
{
    public Text TextGameObject;
    private string text;

    private void Start()
    {
        text = TextGameObject.text;
        TextGameObject.text = "";
        StartCoroutine(TextCorutine());
    }


    IEnumerator TextCorutine()
    {
        foreach (var symbol in text)
        {
            TextGameObject.text += symbol;
            yield return new WaitForSeconds(0.075f);
        }
    }
}