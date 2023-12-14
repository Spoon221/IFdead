using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class LobbyBoardController : MonoBehaviour
{
    [SerializeField] public TMP_Text versionText;
    [SerializeField] public TMP_Text serverText;

    [SerializeField] private RectTransform manObject;
    [SerializeField] private RectTransform survObject;

    private float UISpeed = 0.2f;

    void Start()
    {
        versionText.text = "Версия игры: " + PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion;
        serverText.text = "Регион: " + PhotonNetwork.CloudRegion;
    }


    public void SetRegion(string region)
    {
        serverText.text = "Регион: " + region.ToUpper();
    }

    public void ButtonLeft()
    {
        survObject.DOLocalMoveX(848, UISpeed);
        manObject.DOLocalMoveX(410, UISpeed);
    }

    public void ButtonRight()
    {
        manObject.DOLocalMoveX(-28, UISpeed);
        survObject.DOLocalMoveX(410, UISpeed);
    }
}
