using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    [SerializeField] private Text manaText;
    [SerializeField] private Image manaBar;
    private Stats userStats;
    private float maxUserMana;
    private PhotonView view;

    void Start()
    {
        view = gameObject.GetComponentInParent<PhotonView>();
        if (view.IsMine)
        {
            userStats = gameObject.GetComponentInParent<Stats>();
            userStats.OnManaChanged.AddListener(SetManaBar);
            maxUserMana = userStats.MaxMana;
            SetManaBar(userStats.MaxMana);
        }
    }

    private void SetManaBar(float userMana)
    {
        if (view.IsMine)
        {
            manaText.text = Mathf.Round(userMana).ToString();
            manaBar.fillAmount = userMana / maxUserMana;
        }
        
    }
}