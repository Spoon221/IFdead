using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Item : MonoBehaviour
{
    [field: SerializeField] public string ItemName;
    [HideInInspector] public UnityEvent OnItemPickUp = new UnityEvent();

    public void PickUpItem()
    {
        OnItemPickUp.Invoke();
        Destroy(gameObject);
    }
}
