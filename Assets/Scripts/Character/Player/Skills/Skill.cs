using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [field: SerializeField] public float ManaCost { get; private set; }
    [field: SerializeField] public float CooldownTime { get; private set; }


    public virtual void Activate()
    {
    }
}