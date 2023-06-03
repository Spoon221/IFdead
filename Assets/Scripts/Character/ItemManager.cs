using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class ItemManager : MonoBehaviour
{
    [FormerlySerializedAs("ItemMessage")] [SerializeField]
    private GameObject itemMessage;

    [SerializeField] private TMP_Text actionText;

    public void Start()
    {
        itemMessage.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out PickableItem pickableItem))
        {
            if (!pickableItem.IsCollected)
            {
                actionText.text = $"Подобрать {pickableItem.ItemName}";
                itemMessage.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    pickableItem.PickUpItem();
                    itemMessage.SetActive(false);
                }
            }
        }

        else if (other.TryGetComponent(out ActivatedItem item))
        {
            if (!item.IsActivated)
            {
                actionText.text = item.ItemName;
                itemMessage.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    item.ActivateItem();
                    itemMessage.SetActive(false);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PickableItem pickableItem) || other.TryGetComponent(out ActivatedItem item))
            itemMessage.SetActive(false);
    }
}