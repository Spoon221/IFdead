using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class BotManager : MonoBehaviour
{
    private NavMeshAgent agent;
    private List<Transform> points = new List<Transform>();

    protected enum BotStatus { idle, patrol, pursuit, attaking }
    protected virtual BotStatus botStatus { get; set; }
    protected virtual Transform purposePersecution { get; set; }

    private float timer = 0f;
    private int idleTime = 0;

    //private float softAnimations = 0f;
    private Animator animator;

    void Start()
    {
        EventTime();

        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.ResetPath();

        var objectPoints = GameObject.FindGameObjectWithTag("Points").transform.GetChild(0);
        foreach (Transform t in objectPoints) 
            points.Add(t);
    }

    void Update()
    {
        if (botStatus == BotStatus.idle)
        {
            timer += Time.deltaTime;

            if (timer > idleTime)
            {
                timer = 0f;
                EventTime();
                botStatus = BotStatus.patrol;
                var pos = Random.Range(0, points.Count);
                var path = new NavMeshPath();
                agent.CalculatePath(points[pos].position, path);
                agent.SetPath(path);
                animator.SetFloat("motion", 1);
            }
        }

        if (botStatus == BotStatus.patrol && agent.remainingDistance - agent.stoppingDistance < 1e-8)
        {
            botStatus = BotStatus.idle;
            animator.SetFloat("motion", 0);
        }

        if (botStatus == BotStatus.pursuit)
        {
            animator.SetFloat("motion", 1);
            agent.SetDestination(purposePersecution.position);
        }
    }

    private void EventTime()
    {
        idleTime = Random.Range(0, 12);
    }
}
