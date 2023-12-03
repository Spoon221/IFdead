using Items;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Photon.Pun;

public class Generator : ActivatedItem
{
    public float baseRepairTime = 3f; // Базовое время, необходимое для починки генератора

    [FormerlySerializedAs("generatorHealth")]
    public float baseGeneratorHealth = 1000f;

    private SingletonGeneratorHealth singltonGeneratorHealth;

    private int playerCount; // Количество игроков в триггере

    private float tickGeneratorRepairing; // Время тика починки генератора
    private bool isRepairing;
    public int CounterCompletedTasks;

    private bool isRepaired;

    public UnityEvent isPlayerInTriggerZone = new();
    public UnityEvent isPlayerExitTriggerZone = new();

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerCount++;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        OnTriggerEnterPlayer();
        
        //Debug.Log(singltonGeneratorHealth.GetHealth());
        if (!isRepaired)
        {
            singltonGeneratorHealth.AddHealth(tickGeneratorRepairing * playerCount);
        }

        if (singltonGeneratorHealth.GetHealth() >= baseGeneratorHealth && !isRepaired)
        {
            //Debug.Log("loaded");
            photonView.RPC("SuncComplitedTask", RpcTarget.All);
        }
    }

    private void OnTriggerEnterPlayer()
    {
        isPlayerInTriggerZone.Invoke();
    }

    private void OnTriggerExitPlayer()
    {
        isPlayerExitTriggerZone.Invoke();
    }
    private void OnTriggerExit(Collider other)
    {
        OnTriggerExitPlayer();
        if (other.gameObject.CompareTag("Player"))
        {
            playerCount--;
        }
    }


    public override void Start()
    {
        CounterCompletedTasks = 0;
        singltonGeneratorHealth = SingletonGeneratorHealth.GetInstance();
        tickGeneratorRepairing = baseGeneratorHealth / baseRepairTime / 1000;
    }

    [PunRPC]
    public void SuncComplitedTask()
    {
        isRepaired = true;
        ActivateItem();
        CounterCompletedTasks = 1;
    }
}