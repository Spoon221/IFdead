using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class ManiacMinigame : MonoBehaviourPunCallbacks
{
    private PlayerMinigame caughtPlayer;
    private int _rescueProgress;
    private Image progressBar;
    private RectTransform keyRect;
    private GameObject canvas;

    private ManiacMovementController movementController;

    public static readonly KeyCode[] validSequenceKeys = {
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

    public int RescueProgress
    {
        get => _rescueProgress;
        //set => SyncVal(value);
        set => photonView.RPC("SyncVal", RpcTarget.All, value);
    }

    public bool Mine => photonView.IsMine;

    [PunRPC]
    private void SyncVal(int newVal)
    {
        _rescueProgress = newVal;
    }

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
        var viewID = playerCollider.gameObject.GetPhotonView().ViewID;
        photonView.RPC("StartMiniGameRPC", RpcTarget.All, viewID);
        //StartMiniGameRPC(playerCollider);
    }

    [PunRPC]
    private void StartMiniGameRPC(int viewID)
    {
        RescueProgress = 50;

        var playerGM = PhotonNetwork.GetPhotonView(viewID).gameObject;

        if (!playerGM.TryGetComponent(out caughtPlayer)) return;
        if (!photonView.IsMine) return;
        StartCoroutine(QTEGame());
        movementController.canMove = false;
        caughtPlayer.StartMiniGameRPCSupport();

        progressBar.fillAmount = RescueProgress / 100f;
        canvas.gameObject.SetActive(true);
    }
    [PunRPC]
    private void ReleasePlayer()
    {
        //if (!photonView.IsMine) return;
        Debug.Log("Got rel");
        movementController.canMove = true;
        StopAllCoroutines();
        caughtPlayer.Release();
        canvas.SetActive(false);
        caughtPlayer = null;
        progressBar.fillAmount = 50;
        RescueProgress = 50;
        GetComponent<ManiacHook>().Miss.UnHook();
        StartCoroutine(Stun());
    }

    private IEnumerator Stun()
    {
        movementController.canMove = false;
        yield return new WaitForSeconds(4);
        movementController.canMove = true;
    }

    [PunRPC]
    private void KillPlayer()
    {
        //if (!photonView.IsMine) return;
        Debug.Log("Got kill");
        movementController.canMove = true;
        StopAllCoroutines();
        caughtPlayer.Kill();
        canvas.SetActive(false);
        caughtPlayer = null;
        RescueProgress = 50;
        progressBar.fillAmount = 50;
        GetComponent<ManiacHook>().Miss.UnHook();
    }
    private void FixedUpdate()
    {
        if (caughtPlayer == null) return;

        if (RescueProgress < 0) RescueProgress = 0;
        progressBar.fillAmount = RescueProgress / 100f;

        if (RescueProgress >= 100)
        {
            photonView.RPC("ReleasePlayer", RpcTarget.All);
            Debug.Log("Call rel");
        }
        else if (RescueProgress <= 0)
        {

            photonView.RPC("KillPlayer", RpcTarget.All);
            Debug.Log("Call kill");
        }
    }

    private IEnumerator QTEGame()
    {
        while (caughtPlayer is not null)
        {
            var rand = Random.Range(0, validSequenceKeys.Length-1);
            SetKeyOnScreen(validSequenceKeys[rand]);
            yield return new WaitUntil(() => Input.GetKeyDown(validSequenceKeys[rand]));
            RescueProgress -= 10;
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
