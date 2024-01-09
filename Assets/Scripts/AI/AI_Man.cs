using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class AI_Man : MonoBehaviour
{
    private NavMeshAgent agent;
    public LayerMask layerMask;
    public Transform[] SurvPlayers = {};


    private int timer;

    private Transform[] generators => AI_GAME.generators;
    private Transform[] exits => AI_GAME.exits;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        SurvPlayers = Array.ConvertAll(GameObject.FindGameObjectsWithTag("Survival"), item => item.transform);
    }

    void FixedUpdate()
    {
        var surv = SurvPlayers[FindNearestTrans(SurvPlayers)];
        if (CanSeeSurv(surv) || timer > 0)
        {
            agent.SetDestination(surv.position);
            timer--;
        }
        else
        {
            WalkToNearest_Generator();
        }
    }

    private bool CanSeeSurv(Transform surv)
    {
        
        if (Vector3.Distance(surv.position, transform.position) > 20) return false;

        Ray ray = new(transform.position + new Vector3(0, 1, 0), surv.position - new Vector3(0, 1, 0) - transform.position);
        Debug.DrawRay(transform.position + new Vector3(0, 1, 0), surv.position - new Vector3(0, 1, 0) - transform.position, Color.red);
        Physics.Raycast(ray, out var hit, layerMask);
        if (hit.collider == null) return false;

        if (hit.collider?.tag != "Survival") return false;
        timer = 5 * 40;
        return true;

    }

    private int nowGen;
    public void WalkToNearest_Generator()
    {
        //agent.stoppingDistance = 2;
        if(nowGen ==-1)
            nowGen = FindNearestTrans(generators);
        if(Vector3.Distance(transform.position, generators[nowGen].position) < 2)
            nowGen = Random.Range(0,generators.Length);
        agent.SetDestination(generators[nowGen].position);
        agent.isStopped = false;
    }

    public int FindNearestTrans(Transform[] points)
    {
        var nearest = 0;

        for (var i = 1; i < points.Length; i++)
        {
            if (Vector3.Distance(transform.position, points[i].position) < Vector3.Distance(transform.position, points[nearest].position))
            {
                nearest = i;
            }
        }

        return nearest;
    }
}
