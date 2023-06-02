using System;
using UnityEngine;

public class AI_FieldOfView : AI_Manager
{
    //[SerializeField] private float RadiusFieldsView;
    public float RadiusFieldsView;
    [Range(0, 360)] public float AngleView;
    [SerializeField] private LayerMask TargetMask;

    //private bool canSeePlayer;
    public bool canSeePlayer;

    private Collider[] objectsArea;

    private void FixedUpdate()
    {
        FieldView();
    }

    public void CheckingFieldView()
    {
        if (canSeePlayer)
        {
            botStatus = BotStatus.chase;
        }
    }

    private void FieldView()
    {
        objectsArea = Physics.OverlapSphere(transform.position, RadiusFieldsView, TargetMask);

        if (objectsArea.Length != 0)
        {
            var target = objectsArea[0].transform;
            var directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < AngleView / 2)
            {
                canSeePlayer = true;
                purposePersecution = target.transform.parent.gameObject.transform;
            }
            else
            {
                canSeePlayer = false;
            }
        }
        else if (canSeePlayer)
        {
            purposePersecution = null;
            canSeePlayer = false;
        }
    }
}