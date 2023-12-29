using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerMinigame : MonoBehaviourPunCallbacks
{
    private bool isCaught;
    private ManiacMinigame maniac;
    private Coroutine routine;
    private RectTransform keyRect;

    public void Start()
    {
        keyRect = MiniGameCanvas.KeyRect;
    }
    public void StartMiniGameRPCSupport()
    {
        photonView.RPC("StartMiniGameRPC", RpcTarget.All);
    }
    [PunRPC]
    public void StartMiniGameRPC()
    {
        if(!photonView.IsMine) return;
        //photonView.RPC("StartMiniGame", RpcTarget.All, mGame);
        var mGame = GameObject.FindGameObjectWithTag("Maniac").GetComponent<ManiacMinigame>();
        StartMiniGame(mGame);
    }
    private void StartMiniGame(ManiacMinigame mGame)
    {
        maniac = mGame;
        isCaught = true;
        GetComponent<PlayerMovementController>().canMove = false;
        routine = StartCoroutine(QTEGame());
        GetComponentInChildren<Transform>().Find("survivor").GetComponent<Animator>().SetFloat("FrontMove", 0);
    }

    private IEnumerator QTEGame()
    {
        while (isCaught)
        {
            var rand = Random.Range(0, ManiacMinigame.validSequenceKeys.Length - 1);
            SetKeyOnScreen(ManiacMinigame.validSequenceKeys[rand]);
            yield return new WaitUntil(() => Input.GetKeyDown(ManiacMinigame.validSequenceKeys[rand]));

            maniac.RescueProgress += 20;
            yield return new WaitForEndOfFrame();
        }
    }
    private void SetKeyOnScreen(KeyCode key)
    {
        const int frameWidth = 5;
        var yPos = Random.Range(Screen.height / frameWidth, Screen.height - Screen.height / frameWidth);
        var xPos = Random.Range(Screen.width / frameWidth, Screen.width - Screen.width / frameWidth);


        keyRect.anchoredPosition = new Vector2(xPos, yPos);
        keyRect.GetComponentInChildren<Text>().text = key.ToString();
    }

    internal void Release()
    {
        if(routine != null )
            StopCoroutine(routine);
        isCaught = false;
        maniac = null;
        GetComponent<PlayerMovementController>().canMove = true;
    }

    public void Kill()
    {
        if (routine != null)
            StopCoroutine(routine);
        isCaught = false;
        maniac = null;
        GetComponent<PlayerMovementController>().canMove = true;
        GetComponent<PlayerStats>().GetDamage(100000);
    }
}
