using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Photon.Pun;

public class ItemManager : MonoBehaviourPunCallbacks
{
    [FormerlySerializedAs("ItemMessage")]
    [SerializeField] private GameObject itemMessage;
    [SerializeField] private TMP_Text actionText;
    public PickableItem pickableItem;

    public void Start()
    {
        itemMessage.SetActive(false);
        pickableItem = FindObjectOfType(typeof(PickableItem)) as PickableItem;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out PickableItem pickableItem))
        {
            if (!pickableItem.IsCollected)
            {
                actionText.text = $"Подобрать {pickableItem.ItemName}";
                itemMessage.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E) && pickableItem.view.IsMine)
                {
                    //pickableItem.PickUpItem();
                    pickableItem.view.RPC("PickUpItem", RpcTarget.All);
                    itemMessage.SetActive(false);
                }
            }
        }

        //else if (other.TryGetComponent(out ActivatedItem item))
        //{
        //    if (!item.IsActivated)
        //    {
        //        actionText.text = item.ItemName;
        //        itemMessage.SetActive(true);
        //        if (Input.GetKeyDown(KeyCode.E) || !photonView.IsMine) ///подумать над реализацией в онлайне
        //        {
        //            item.ActivateItem();
        //            itemMessage.SetActive(false);
        //        }
        //    }
        //}
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PickableItem pickableItem) || other.TryGetComponent(out ActivatedItem item))
            itemMessage.SetActive(false);
    }
}