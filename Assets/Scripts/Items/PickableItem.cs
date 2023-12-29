using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

public class PickableItem : MonoBehaviourPunCallbacks
{
    [field: SerializeField] public string ItemName { get; private set; }
    public bool IsCollected { get; private set; }
    [HideInInspector] public UnityEvent OnItemPickUp = new UnityEvent();
    public PhotonView view;
    public Generator generator;

    public void Start()
    {
        IsCollected = false;
    }

    [PunRPC]
    public void PickUpItem()
    {
        IsCollected = true;
        OnItemPickUp.Invoke();
        generator.CounterCompletedTasks += 1;
        Destroy(gameObject);
    }
}