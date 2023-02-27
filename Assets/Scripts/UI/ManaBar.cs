using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    [SerializeField]private PlayerController player;
    [SerializeField] private Text manaText;
    [SerializeField] private Image manaBar;
    private int maxPlayerMana;

    void Start()
    {
        player.OnManaChanged.AddListener(SetManaBar);
        maxPlayerMana = player.MaxHealth;
        SetManaBar(player.MaxMana);
    }

    private void SetManaBar(float playerMana)
    {
        manaText.text = Mathf.Round(playerMana).ToString()  ;
        manaBar.fillAmount = (float) playerMana / maxPlayerMana;
    }
}