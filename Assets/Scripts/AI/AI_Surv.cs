using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class AI_Surv : MonoBehaviour
{
    private NavMeshAgent agent;

    public Transform[] generators;
    public Transform[] exits;


    public Transform TestPoint;
    public Transform TestPoint2;

    public Transform maniac;

    private int pathRadius = 25;

    public LayerMask layerMask;

    private bool _isRunning;

    private int viewRange = 10;

    public bool IsRunning
    {
        get => _isRunning;
        set
        {
            if (value)
            {
                timer = 5 * 40;
            }

            _isRunning = value;
        }
    }

    public int timer;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        WalkToAny_Generator();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (CanSeeManiac())
        {
            IsRunning = true;
            viewRange = 20;
            RunForYourLife();
        }
        if (timer > 0)
        {
            RunForYourLife();
        }
        if(IsRunning)
        {
            timer--;
            if (timer == 0)
            {
                IsRunning = false;
                WalkToNearest_Generator();
                viewRange = 10;
            }
        }
    }

    private void RunForYourLife()
    {
        agent.stoppingDistance = 0;
        var point = GetGoodPoint();
        agent.SetDestination(point);
        

    }

    private Vector3 GetGoodPoint()
    {
        List<Vector3> points = new();

        for (var angel = 0; angel <= 360; angel+=20)
        {
            var point = new Vector3(transform.position.x + pathRadius * Mathf.Cos(angel), 0,
                transform.position.z + pathRadius * Mathf.Sin(angel));
            if (!NavMesh.SamplePosition(point, out var hit, 1000, NavMesh.AllAreas)) continue;
            point = hit.position;
            if (Vector3.Distance(point, maniac.position) < Vector3.Distance(point, transform.position) ||
                Vector3.Distance(point, transform.position) < pathRadius* 0.8f) continue;
            points.Add(point);
        }

        foreach (var vector3 in points)
        {
            Debug.DrawLine(vector3, vector3+ new Vector3(0,1,0),Color.cyan);
        }

        var fathers = 0;
        for (int i = 1; i < points.Count; i++)
        {
            if (Vector3.Distance(maniac.position, points[i]) > Vector3.Distance(maniac.position, points[fathers]))
                fathers = i;
        }


        Debug.DrawLine(points[fathers], points[fathers] + new Vector3(0, 1.5f, 0), Color.magenta);
        return points[fathers];

    }

    private bool CanSeeManiac()
    {
        if (Vector3.Distance(maniac.position, transform.position) > viewRange) return false;

        Ray ray = new(transform.position + new Vector3(0,2,0), maniac.position - new Vector3(0, 1, 0) - transform.position);
        Debug.DrawRay(transform.position + new Vector3(0, 2, 0), maniac.position - new Vector3(0, 1, 0) - transform.position);
        Physics.Raycast(ray, out var hit,layerMask);
        return hit.collider.tag == "Maniac";
    }


    public void WalkToNearest_Generator()
    {
        agent.stoppingDistance = 2;
        var i = FindNearestTrans(generators);
        agent.SetDestination(generators[i].position);
        agent.isStopped = false;
    }

    public void WalkToAny_Generator()
    {
        agent.stoppingDistance = 2;
        var i = Random.Range(0,generators.Length-1);
        agent.SetDestination(generators[i].position);
        agent.isStopped = false;
    }

    public void WalkTo_Exit()
    {
        var i = FindNearestTrans(exits);
        agent.SetDestination(exits[i].position);
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
