using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotAttacking : BotManager
{
    protected override BotStatus botStatus { get; set; }
    protected override Transform purposePersecution { get; set; }

    public float RadiusFieldsView;
    [Range(0, 360)] public float AngleView;
    public LayerMask TargetMask;
    public LayerMask ObstacleMask;
    private bool canSeePlayer;

    private Collider[] objectsArea;

    private void FixedUpdate()
    {
        FieldView();
    }

    private void Update()
    {
        if (canSeePlayer)
        {
            purposePersecution = objectsArea[0].transform;
            botStatus = BotStatus.pursuit;
        }
    }

    private void FieldView()
    {
        objectsArea = Physics.OverlapSphere(transform.position, RadiusFieldsView, TargetMask);

        if (objectsArea.Length != 0)
        {
            var target = objectsArea[0].transform;
            var directionToTarget = (target.position - transform.position).normalized;

            //if (Vector3.Angle(transform.forward, directionToTarget) < AngleView / 2)
            //{
            //    float distanceToTarget = Vector3.Distance(transform.position, target.position);

            //    if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, ObstacleMask))
            //        canSeePlayer = true;

            //    else
            //        canSeePlayer = false;
            //}
            //else
            //    canSeePlayer = false;
        }
        else if (canSeePlayer)
            canSeePlayer = false;
    }
}
