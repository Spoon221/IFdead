using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Text healthText;
    [SerializeField] private Image healthBar;
    private Stats userStats;
    private int maxUserHealth;
    private PhotonView view;

    void Start()
    {
        view = gameObject.GetComponentInParent<PhotonView>();
        if (view.IsMine)
        {
            userStats = gameObject.GetComponentInParent<Stats>();
            userStats.OnHealthChanged.AddListener(SetHealthBar);
            maxUserHealth = userStats.MaxHealth;
            SetHealthBar(userStats.MaxHealth);
        }
    }

    private void SetHealthBar(int userHealth)
    {
        if (view.IsMine)
        {
            healthText.text = userHealth.ToString();
            healthBar.fillAmount = (float) userHealth / maxUserHealth;
        }
       
    }
}