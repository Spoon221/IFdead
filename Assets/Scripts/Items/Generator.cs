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
    private List<GameObject> lights;
    public float baseRepairTime = 3f; // Базовое время, необходимое для починки генератора
    // public float repairTimeMultiplier = 0.5f; // Множитель скорости починки генератора

    [FormerlySerializedAs("generatorHealth")]
    public float baseGeneratorHealth = 1000f;

    // public float generatorCurrentHealth;
    public SingletonGeneratorHealth singltonGeneratorHealth;

    private int playerCount; // Количество игроков в триггере

    private float tickGeneratorRepairing; // Время тика починки генератора
    private bool isRepairing;
    public Slider hp;

    // public TextMeshProUGUI displayText;
    // public GameObject someImage;

    private bool isRepaired;

    public UnityEvent isPlayerInTriggerZone = new();
    public UnityEvent isPlayerExitTriggerZone = new();
    
    // public TMP_Text actionText;
    // public GameObject itemMessage;


    public void OnTriggerEnter(Collider other)
    {
        // someImage.SetActive(true);
        if (other.gameObject.transform.parent.gameObject.CompareTag("Player"))
        {
            playerCount++;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        OnTriggerEnterPlayer();
        Debug.Log("player count:" + playerCount);

        // itemMessage.SetActive(true);
        {
            // displayText.text = (string.Format("Починка генератора: {0}/{1}", (int)(singltonGeneratorHealth.GetHealth()),
                // baseGeneratorHealth));
        }
        Debug.Log(singltonGeneratorHealth.GetHealth());
        if (!isRepaired)
        {
            singltonGeneratorHealth.AddHealth(tickGeneratorRepairing * playerCount);
            // generatorCurrentHealth += (tickGeneratorRepairing * playerCount);
        }

        if (singltonGeneratorHealth.GetHealth() >= baseGeneratorHealth && !isRepaired)
        {
            Debug.Log("fps: ");
            Debug.Log(("Fps: {0}", 1.0f / Time.deltaTime));
            Debug.Log("НАХУЙ ПОЧИНИЛИ СУКА");
            isRepaired = true;
            ActivateItem();
        }
        // Debug.Log(currentRepairTime);
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
        // someImage.SetActive(false);
        // displayText.text = "";
        if (Utils.IsPlayer(other))
        {
            playerCount--;
        }

        Debug.Log("ВЫШЕЛ");
    }


    public void Start()
    {
        
        singltonGeneratorHealth = SingletonGeneratorHealth.GetInstance();
        tickGeneratorRepairing = baseGeneratorHealth / baseRepairTime / 1000;
    }

    public override void ActivateItem()
    {
        base.ActivateItem();
    }
}