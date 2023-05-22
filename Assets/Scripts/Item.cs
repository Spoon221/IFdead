using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

public class Item : MonoBehaviour
{
    [field: SerializeField] public string ItemName { get; private set; }
    public bool Collected { get; private set; } = false;
    [HideInInspector] public UnityEvent OnItemPickUp = new UnityEvent();


    public void Start()
    {
        Collected = false;
    }

    public void PickUpItem()
    {
        Collected = true;
        OnItemPickUp.Invoke();
        PhotonNetwork.Destroy(gameObject);
    }
}
