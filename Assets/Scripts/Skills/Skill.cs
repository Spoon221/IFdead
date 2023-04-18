using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : ScriptableObject
{
    public new string name;
    public float cooldownTime;
    public float activeTime;

    public virtual void Activate(GameObject parent)
    {
    }
}