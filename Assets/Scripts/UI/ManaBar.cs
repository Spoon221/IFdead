using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    [SerializeField] private Text manaText;
    [SerializeField] private Image manaBar;
    private PlayerStats player;
    private float maxPlayerMana;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        player.OnManaChanged.AddListener(SetManaBar);
        maxPlayerMana = player.MaxMana;
        SetManaBar(player.MaxMana);
    }

    private void SetManaBar(float playerMana)
    {
        manaText.text = Mathf.Round(playerMana).ToString();
        manaBar.fillAmount = playerMana / maxPlayerMana;
    }
}