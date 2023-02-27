using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]private PlayerController player;
    [SerializeField] private Text healthAmount;
    [SerializeField] private Image healthBar;
    
    private int maxPlayerHealth;

    void Start()
    {
        player.OnHealthChanged.AddListener(SetHealthBar);
        maxPlayerHealth = player.MaxHealth;
        SetHealthBar(player.MaxHealth);
    }

    private void SetHealthBar(int playerHealth)
    {
        healthAmount.text = playerHealth.ToString();
        healthBar.fillAmount = (float) playerHealth / maxPlayerHealth;
    }
}