using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class ManiacMinigame : MonoBehaviourPunCallbacks
{
    private PlayerMinigame caughtPlayer;
    public int rescueProgress;
    private Image progressBar;
    private RectTransform keyRect;
    private GameObject canvas;

    public static readonly KeyCode[] validSequenceKeys = new[] {
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

    //private void OnTriggerEnter(Collider other)
    //{
    //    StartMiniGame(other);
    //}

    private void Start()
    {
        canvas = MiniGameCanvas.Canvas;
        keyRect = MiniGameCanvas.KeyRect;
        progressBar = MiniGameCanvas.ProgressBar;
    }

    public void StartMiniGame(Collider playerCollider)
    {
        //if(!PhotonNetwork.IsMasterClient) return;
        //photonView.RPC("StartMiniGameRPC", RpcTarget.All, playerCollider);
        StartMiniGameRPC(playerCollider);
    }

    [PunRPC]
    private void StartMiniGameRPC(Component playerCollider)
    {
        if (!playerCollider.TryGetComponent(out caughtPlayer)) return;
        caughtPlayer.StartMiniGame(this);
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
        GetComponent<ManiacHook>().Miss.UnHook();
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
            yield return new WaitUntil(() => Input.GetKeyDown(validSequenceKeys[rand]) || Input.GetKeyDown(KeyCode.L));
            var dir = transform.position - caughtPlayer.transform.position;
            caughtPlayer.GetComponent<PlayerMovementController>().AddForce(dir * 3);
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
