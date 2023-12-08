using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Items;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

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
            other.GetComponent<IsMine>().GeneratorRect.GetComponent<GeneratorHealthSlider>().DisplaySlider();
        }
        else
            return;
    }

    private void OnTriggerStay(Collider other)
    {
        if (photonView.IsMine)
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
                photonView.RPC("TaskSynchronization", RpcTarget.All);
                photonView.RPC("ActivateItem", RpcTarget.All);
                //ActivateItem();
            }
        }
        else
            return;
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
            other.GetComponent<IsMine>().GeneratorRect.GetComponent<GeneratorHealthSlider>().HideSlider();
        }
        else
            return;
    }


    public override void Start()
    {
        SingletonGeneratorHealth.GetInstance().ResetHealth();
        CounterCompletedTasks = 0;
        singltonGeneratorHealth = SingletonGeneratorHealth.GetInstance();
        tickGeneratorRepairing = baseGeneratorHealth / baseRepairTime / 1000;
    }

    [PunRPC]
    public void TaskSynchronization()
    {
        isRepaired = true;
        CounterCompletedTasks = 1;
    }
}