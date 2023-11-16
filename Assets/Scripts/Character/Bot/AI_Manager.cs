using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class AI_Manager : MonoBehaviourPunCallbacks
{
    private AI_FieldOfView fieldOfView;
    private AI_Attack attack;

    protected static NavMeshAgent agent;
    private List<Transform> points = new List<Transform>();

    protected enum BotStatus { idle, patrol, chase, searchTarget }
    protected static BotStatus botStatus { get; set; }
    protected static Transform purposePersecution { get; set; }
    protected static bool canShot { get; set; }

    private enum TimeEvent { idle, searchTarget }
    private int time;
    private float timer;

    //private float softAnimations = 0f;
    private Animator animator;



    void Start()
    {
        fieldOfView = gameObject.GetComponent<AI_FieldOfView>();
        attack = gameObject.GetComponent<AI_Attack>();

        canShot = true;
        botStatus = BotStatus.idle;
        DurationEvent();

        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.ResetPath();

        var objectPoints = GameObject.FindGameObjectWithTag("Points").transform.GetChild(0);
        foreach (Transform t in objectPoints) 
            points.Add(t);
    }

    void Update()
    {
        fieldOfView.CheckingFieldView();
        StatusLogic();
        attack.CheckingAttackCondition();
    }

    private void StatusLogic()
    {
        if (botStatus == BotStatus.idle)
        {
            timer += Time.deltaTime;

            if (timer > time)
            {
                timer = 0f;
                DurationEvent();
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

        if (botStatus == BotStatus.chase)
        {
            if (purposePersecution == null)
            {
                DurationEvent(TimeEvent.searchTarget);

                var lastPlaceWhereEnemyWasVisible = transform.position + 3 * transform.forward;
                agent.SetDestination(lastPlaceWhereEnemyWasVisible);
                botStatus = BotStatus.searchTarget;
            }
            else
            {
                animator.SetFloat("motion", 1);
                agent.SetDestination(purposePersecution.position);
            }
        }

        if (botStatus == BotStatus.searchTarget && agent.remainingDistance - agent.stoppingDistance < 1e-8)
        {
            timer = 0f;
            botStatus = BotStatus.idle;
            animator.SetFloat("motion", 0);
        }
    }

    private void DurationEvent(TimeEvent name = TimeEvent.idle)
    {
        if (name == TimeEvent.idle)
        {
            time = Random.Range(2, 8);
        }
        if (name == TimeEvent.searchTarget)
        {
            time = Random.Range(6, 14);
        }
    }
}