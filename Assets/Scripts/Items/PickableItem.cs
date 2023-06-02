using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

public class PickableItem : MonoBehaviour
{
    [field: SerializeField] public string ItemName { get; private set; }
    public bool IsCollected { get; private set; }
    [HideInInspector] public UnityEvent OnItemPickUp = new UnityEvent();


    public void Start()
    {
        IsCollected = false;
    }

    public void PickUpItem()
    {
        IsCollected = true;
        OnItemPickUp.Invoke();
        Destroy(gameObject);
    }
}