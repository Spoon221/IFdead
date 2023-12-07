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
            other.GetComponentInChildren<GeneratorHealthSlider>().DisplaySlider();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        OnTriggerEnterPlayer();
        
        Debug.Log(singltonGeneratorHealth.GetHealth());
        if (!isRepaired)
        {
            singltonGeneratorHealth.AddHealth(tickGeneratorRepairing * playerCount);
        }

        if (singltonGeneratorHealth.GetHealth() >= baseGeneratorHealth && !isRepaired)
        {
            Debug.Log("loaded");
            isRepaired = true;
            ActivateItem();
            CounterCompletedTasks = 1;
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
            other.GetComponentInChildren<GeneratorHealthSlider>().HideSlider();
        }
    }


    public override void Start()
    {
        CounterCompletedTasks = 0;
        singltonGeneratorHealth = SingletonGeneratorHealth.GetInstance();
        tickGeneratorRepairing = baseGeneratorHealth / baseRepairTime / 1000;
    }
}