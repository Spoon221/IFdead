using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;

public class ExitForPlayer : MonoBehaviour
{
    [SerializeField] private Settings settings;
    [SerializeField] private Generator generator;
    [SerializeField] private GameObject WinnerCanvas;
    private MeshRenderer meshRenderer;
    private BoxCollider boxCollider;
    private int complitedTask;
    private float displayTime = 5f;

    private void Start()
    {
        WinnerCanvas.SetActive(false);
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = false;
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.isTrigger = false;
        complitedTask = generator.CounterCompletedTasks;
    }

    private void Update()
    {
        complitedTask = generator.CounterCompletedTasks;
        if (complitedTask == 2)
        {
            boxCollider.isTrigger = true;
            meshRenderer.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (settings.photonView.IsMine && other.CompareTag("Player"))
        {
            StartCoroutine(ShowCanvasAndLeaveGame());
        }
        else
        {
            StartCoroutine(CheckPlayersAfterCoroutine());
        }
    }

    public IEnumerator ShowCanvasAndLeaveGame()
    {
        WinnerCanvas.SetActive(true);
        yield return new WaitForSeconds(displayTime);
        settings.LeaveGame();
        WinnerCanvas.SetActive(false);

        StartCoroutine(CheckPlayersAfterCoroutine());
    }

    private IEnumerator CheckPlayersAfterCoroutine()
    {
        yield return new WaitForEndOfFrame();

        var countPlayers = GameObject.FindGameObjectsWithTag("Player").Length;
        if (countPlayers < 1)
            settings.view.RPC("LeaveGame", RpcTarget.All);
    }
}
