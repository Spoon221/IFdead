using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerMinigame : MonoBehaviour
{
    private bool isCaught;
    private ManiacMinigame maniac;
    private Coroutine routine;
    private RectTransform keyRect;

    public void Start()
    {
        keyRect = MiniGameCanvas.KeyRect;
    }
    public void StartMiniGame(ManiacMinigame mGame)
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
            //SetKeyOnScreen(ManiacMinigame.validSequenceKeys[rand]);
            yield return new WaitUntil(() => Input.GetKeyDown(ManiacMinigame.validSequenceKeys[rand]) || Input.GetKeyDown(KeyCode.P));

            maniac.rescueProgress += 20;
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
        StopCoroutine(routine);
        isCaught = false;
        maniac = null;
        GetComponent<PlayerMovementController>().canMove = true;
    }
}
