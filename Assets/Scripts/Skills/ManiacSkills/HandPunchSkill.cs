using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New handPunch skill", menuName = "Skills/Maniac")]
public class HandPunchSkill : Skill
{
    [SerializeField] private float damage;
    [SerializeField] private float distance;
    [SerializeField] private GameObject handModel;

    public override void Activate(GameObject parent)
    {
        var x = parent.GetComponent<Transform>();
        
    }

}
