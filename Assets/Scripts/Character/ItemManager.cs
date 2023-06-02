using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out PickableItem pickableItem))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (!pickableItem.IsCollected)
                    pickableItem.PickUpItem();
            }
        }

        else if (other.TryGetComponent(out ActivatedItem item))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (!item.IsActivated)
                    item.ActivateItem();
            }
        }
    }
}