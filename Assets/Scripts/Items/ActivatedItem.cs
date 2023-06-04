using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

public class ActivatedItem : MonoBehaviour
{
    [field: SerializeField] public string ItemName { get; private set; }
    public bool IsActivated { get; private set; }
    [HideInInspector] public UnityEvent OnItemActivate = new UnityEvent();
    private Material outlineMaterial;


    public virtual void Start()
    {
        IsActivated = false;
    }

    public virtual void ActivateItem()
    {
        IsActivated = true;
        OnItemActivate.Invoke();
    }
    
}