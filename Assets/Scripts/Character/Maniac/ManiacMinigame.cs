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

    private ManiacMovementController movementController;

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
        movementController = GetComponent<ManiacMovementController>();
    }

    public void StartMiniGame(Component playerCollider)
    {
        if (!PhotonNetwork.IsMasterClient) return;
        photonView.RPC("StartMiniGameRPC", RpcTarget.All, playerCollider);
        //StartMiniGameRPC(playerCollider);
    }

    [PunRPC]
    private void StartMiniGameRPC(Component playerCollider)
    {
        if (!playerCollider.TryGetComponent(out caughtPlayer)) return;
        caughtPlayer.StartMiniGame(this);
        StartCoroutine(QTEGame());
        rescueProgress = 50;
        progressBar.fillAmount = rescueProgress / 100f;
        canvas.gameObject.SetActive(true);
    }

    private void ReleasePlayer()
    {
        caughtPlayer.Release();
        canvas.gameObject.SetActive(false);
        caughtPlayer = null;
        progressBar.fillAmount = 50;
        GetComponent<ManiacHook>().Miss.UnHook();
        StartCoroutine(Stun());
    }

    private IEnumerator Stun()
    {
        movementController.canMove = false;
        yield return new WaitForSeconds(4);
        movementController.canMove = true;
    }


    private void KillPlayer()
    {
        caughtPlayer.Kill();
        canvas.gameObject.SetActive(false);
        caughtPlayer = null;
        progressBar.fillAmount = 50;
        GetComponent<ManiacHook>().Miss.UnHook();
    }
    private void FixedUpdate()
    {
        if(caughtPlayer is null) return;

        if (rescueProgress < 0) rescueProgress = 0;
        progressBar.fillAmount = rescueProgress / 100f;

        if (rescueProgress >= 100) ReleasePlayer();
        else if (rescueProgress <= 0) KillPlayer();
    }

    private IEnumerator QTEGame()
    {
        while (caughtPlayer is not null)
        {
            var rand = Random.Range(0, validSequenceKeys.Length-1);
            SetKeyOnScreen(validSequenceKeys[rand]);
            yield return new WaitUntil(() => Input.GetKeyDown(validSequenceKeys[rand]) || Input.GetKeyDown(KeyCode.L));
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
