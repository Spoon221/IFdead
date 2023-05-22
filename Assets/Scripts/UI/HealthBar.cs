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
    private Stats userStats;
    private int maxUserHealth;
    public PhotonView UserView;

    void Start()
    {
        if (UserView.IsMine)
        {
            userStats = gameObject.GetComponentInParent<Stats>();
            userStats.OnHealthChanged.AddListener(SetHealthBar);
            maxUserHealth = userStats.MaxHealth;
            SetHealthBar(userStats.MaxHealth);
        }
    }

    private void SetHealthBar(int userHealth)
    {
        if (UserView.IsMine)
        {
            healthText.text = userHealth.ToString();
            healthBar.fillAmount = (float) userHealth / maxUserHealth;
        }
       
    }
}