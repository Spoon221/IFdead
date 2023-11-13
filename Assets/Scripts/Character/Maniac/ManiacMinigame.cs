using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManiacMinigame : MonoBehaviour
{
    private PlayerMinigame caughtPlayer;
    public int rescueProgress;
    public Image progressBar;
    public RectTransform keyRect;
    public Canvas canvas;

    public static KeyCode[] validSequenceKeys = new[] {
        KeyCode.Q,
        KeyCode.W,
        KeyCode.E,
        KeyCode.R,
        KeyCode.T,
        KeyCode.A,
        KeyCode.S,
        KeyCode.D,
        KeyCode.F,
        KeyCode.G,
        KeyCode.Z,
        KeyCode.X,
        KeyCode.C,
        KeyCode.V,
        KeyCode.B,
    };

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out caughtPlayer)) return;
        caughtPlayer.StartMinigame(this);
        StartCoroutine(QTEGame());
        rescueProgress = 0;
        canvas.gameObject.SetActive(true);

    }

    private void ReleasePlayer()
    {
        caughtPlayer.Release();
        canvas.gameObject.SetActive(false);
        caughtPlayer = null;
        progressBar.fillAmount = 0;
    }
    private void FixedUpdate()
    {
        if(caughtPlayer is null) return;

        if (rescueProgress < 0) rescueProgress = 0;
        progressBar.fillAmount = rescueProgress / 100f;

        if (rescueProgress >= 100) ReleasePlayer();
    }

    private IEnumerator QTEGame()
    {
        while (caughtPlayer is not null)
        {
            var rand = Random.Range(0, validSequenceKeys.Length-1);
            SetKeyOnScreen(validSequenceKeys[rand]);
            yield return new WaitUntil(() => Input.GetKeyDown(validSequenceKeys[rand]));

            rescueProgress -= 10;
            yield return new WaitForEndOfFrame();
        }
    }
    private void SetKeyOnScreen(KeyCode key)
    {
        const int frameWidth = 5;
        var yPos = Random.Range(Screen.height / frameWidth, Screen.height - Screen.height / frameWidth);
        var xPos = Random.Range(Screen.width / frameWidth, Screen.width - Screen.width / frameWidth);


        keyRect.anchoredPosition = new Vector2(xPos,yPos);
        keyRect.GetComponentInChildren<Text>().text = key.ToString();
    }
}
