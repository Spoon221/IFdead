using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class AI_Behaviour : MonoBehaviour
{
    private NavMeshAgent agent;
    [Range(0, 360)] public float AngleView;
    private Animator animator;

    public bool canSeePlayer;
    public bool canShot = true;


    private Transform chaseTarget;


    [SerializeField] private float distanceAttack;
    public Transform[] navPoints;
    [SerializeField] private Missile prefabShot;
    public float RadiusFieldsView;
    [SerializeField] private LayerMask TargetMask;


    private void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        LostPlayer();
    }

    private void FixedUpdate()
    {
        FieldView();
        ÑheckingAttackCondition();
    }

    private IEnumerator ChasePlayer()
    {
        animator.SetFloat("motion", 1);
        agent.isStopped = false;
        while (canSeePlayer)
        {
            agent.destination = chaseTarget.position;
            animator.SetFloat("motion",
                Vector3.Distance(agent.destination, transform.position) <= agent.stoppingDistance ? 0 : 1);
            yield return new WaitForFixedUpdate();
        }

        LostPlayer();
    }

    public void LostPlayer()
    {
        StartCoroutine(WalkingToPoints());
    }

    private IEnumerator WalkingToPoints()
    {
        //var pointIndex = FindNearestPoint(transform.position, navPoints);
        while (true)
        {
            var pointIndex = Random.Range(0, navPoints.Length - 1);
            agent.isStopped = false;
            animator.SetFloat("motion", 1);
            agent.destination = navPoints[pointIndex].position;
            yield return new WaitUntil(
                () => Vector3.Distance(agent.destination, transform.position) < 2 || canSeePlayer);
            agent.isStopped = true;
            animator.SetFloat("motion", 0);
            yield return new WaitForDone(5, () => canSeePlayer);
            if (!canSeePlayer) continue;
            break;
        }
    }

    //public static int FindNearestPoint(Vector3 position, Transform[] navPoints)
    //{
    //    int nearIndex;
    //    for (int i = 0; i < UPPER; i++)
    //    {
    //        if (Vector3.Distance(position, navPoint.position) < Vector3.Distance(position, near.position))
    //    }
    //}

    private void FieldView()
    {
        var objectsArea = Physics.OverlapSphere(transform.position, RadiusFieldsView, TargetMask);

        if (objectsArea.Length != 0)
        {
            var target = objectsArea[0].transform;
            var directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < AngleView / 2)
            {
                canSeePlayer = true;
                chaseTarget = target.transform.parent.gameObject.transform;
                animator.SetFloat("motion", 1);
                StartCoroutine(ChasePlayer());
            }
            else
            {
                canSeePlayer = false;
            }
        }
        else if (canSeePlayer)
        {
            chaseTarget = null;
            canSeePlayer = false;
        }
    }

    public void ÑheckingAttackCondition()
    {
        if (chaseTarget is null) return;
        if (canShot
            && Vector3.Distance(transform.position, chaseTarget.position) < distanceAttack)
            Shot();
    }

    [PunRPC]
    public void Shot()
    {
        Instantiate(prefabShot, new Vector3(transform.position.x, 0.5f, transform.position.z),
            transform.rotation);
        canShot = false;
        StartCoroutine(RechargeGun());
    }

    private IEnumerator RechargeGun()
    {
        yield return new WaitForSeconds(prefabShot.CooldownTime);
        canShot = true;
    }

    public sealed class WaitForDone : CustomYieldInstruction
    {
        private readonly Func<bool> m_Predicate;
        private float m_timeout;

        public WaitForDone(float timeout, Func<bool> predicate)
        {
            m_Predicate = predicate;
            m_timeout = timeout;
        }

        public override bool keepWaiting => !WaitForDoneProcess();

        private bool WaitForDoneProcess()
        {
            m_timeout -= Time.deltaTime;
            return m_timeout <= 0f || m_Predicate();
        }
    }
}