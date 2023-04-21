using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Text healthText;
    [SerializeField] private Image healthBar;
    private PlayerStats player;
    private int maxPlayerHealth;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        player.OnHealthChanged.AddListener(SetHealthBar);
        maxPlayerHealth = player.MaxHealth;
        SetHealthBar(player.MaxHealth);
    }

    private void SetHealthBar(int playerHealth)
    {
        healthText.text = playerHealth.ToString();
        healthBar.fillAmount = (float) playerHealth / maxPlayerHealth;
    }
}