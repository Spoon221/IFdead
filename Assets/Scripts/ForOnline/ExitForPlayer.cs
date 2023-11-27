using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ExitForPlayer : MonoBehaviour
{
    [SerializeField] private Settings settings;
    [SerializeField] private PhotonView view;
    [SerializeField] private Generator generator;
    private MeshRenderer meshRenderer;
    private BoxCollider boxCollider;
    private int complitedTask;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = false;
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.isTrigger = false;
        complitedTask = generator.CounterCompletedTasks;
    }

    private void Update()
    {
        complitedTask = generator.CounterCompletedTasks;
        if (complitedTask >= 1)
        {
            boxCollider.isTrigger = true;
            meshRenderer.enabled = true;
        }
        var countPlayers = GameObject.FindGameObjectsWithTag("Player").Length;
        if (countPlayers == 0)
            settings.view.RPC("LeaveGame", RpcTarget.All);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            settings.LeaveGame();
    }
}
