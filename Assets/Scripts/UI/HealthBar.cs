using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Text healthText;
    [SerializeField] private Image healthBar;
    private PlayerStats userStats;
    private int maxUserHealth;
    public PhotonView view;

    void Start()
    {
        if (view.IsMine)
        {
            userStats = gameObject.GetComponentInParent<PlayerStats>();
            userStats.OnHealthChanged.AddListener(SetHealthBar);
            maxUserHealth = userStats.MaxHealth;
            SetHealthBar(userStats.MaxHealth);
        }
    }

    private void SetHealthBar(int userHealth)
    {
        healthText.text = userHealth.ToString();
        healthBar.fillAmount = (float) userHealth / maxUserHealth;
    }
}