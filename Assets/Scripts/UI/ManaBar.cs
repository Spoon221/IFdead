using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    [SerializeField] private Text manaText;
    [SerializeField] private Image manaBar;
    private Stats userStats;
    private float maxUserMana;

    void Start()
    {
        var survivor = GameObject.FindGameObjectWithTag("Player");
        userStats = survivor != null ? survivor.GetComponent<Stats>() : GameObject.FindGameObjectWithTag("Maniac").GetComponent<Stats>();
        userStats.OnManaChanged.AddListener(SetManaBar);
        maxUserMana = userStats.MaxMana;
        SetManaBar(userStats.MaxMana);
    }

    private void SetManaBar(float userMana)
    {
        manaText.text = Mathf.Round(userMana).ToString();
        manaBar.fillAmount = userMana / maxUserMana;
    }
}