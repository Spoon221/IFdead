using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Photon.Pun;

public class ItemManager : MonoBehaviourPun
{
    [FormerlySerializedAs("ItemMessage")]
    [SerializeField]
    private GameObject itemMessage;

    [SerializeField] private TMP_Text actionText;

    [SerializeField] private PhotonView view;
    private PickableItem pickableItem;

    public void Start()
    {
        view = PhotonView.Get(this);
        itemMessage.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out PickableItem pickableItem))
        {
            if (!pickableItem.IsCollected)
            {
                this.pickableItem = pickableItem;
                actionText.text = $"Подобрать {pickableItem.ItemName}";
                //itemMessage.SetActive(true);
                //Key(pickableItem);
                //view.RPC("Key", RpcTarget.AllBuffered);
                pickableItem.PickUpItem();
                //itemMessage.SetActive(false);
            }
        }

        // else if (other.TryGetComponent(out ActivatedItem item))
        // {
            // if (!item.IsActivated)
            // {
                // actionText.text = item.ItemName;
                // itemMessage.SetActive(true);
                // if (Input.GetKeyDown(KeyCode.E) || !photonView.IsMine) ///подумать над реализацией в онлайне
                // {
                    // item.ActivateItem();
                    // itemMessage.SetActive(false);
                // }
            // }
        // }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PickableItem pickableItem))
            itemMessage.SetActive(false);
    }

    //[PunRPC]
    //void Key()
    //{
    //    pickableItem.PickUpItem();
    //}
}