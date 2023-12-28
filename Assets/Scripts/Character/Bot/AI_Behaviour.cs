using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class AI_Behaviour : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;

    [Range(0, 360)] public float AngleView;
    [SerializeField] private LayerMask viewRaycastLayerMask;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float radiusFieldsView;
    [SerializeField] private float distanceAttack;

    public Transform[] navPoints;
    [SerializeField] private Missile prefabShot;
    [SerializeField] private Light light;
    private Material eyeMaterial;

    public bool canSeePlayer;
    private bool canShot = true;
    private Transform chaseTarget;




    private void Start()
    {
        eyeMaterial = light.GetComponentInParent<MeshRenderer>().material;
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        LostPlayer();
    }

    private void FixedUpdate()
    {
        FieldView();
        CheckingAttackCondition();
    }

    private IEnumerator ChasePlayer()
    {

        StartWalkAnim();
        agent.isStopped = false;
        light.color = Color.red;
        eyeMaterial.SetColor("_Emission", Color.red);
        while (canSeePlayer)
        {
            agent.destination = chaseTarget.position;
            if (Vector3.Distance(agent.destination, transform.position) <= agent.stoppingDistance)
            {
                StopWalkAnim();
            }
            else
            {
                StartWalkAnim();
            }
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitUntil(() => (canSeePlayer || Vector3.Distance(agent.destination, transform.position) <= agent.stoppingDistance));
        StopWalkAnim();
        light.color = Color.yellow;
        eyeMaterial.SetColor("_Emission", Color.yellow);
        yield return new WaitForDone(3, () => canSeePlayer);
        if (!canSeePlayer)
            LostPlayer();
        else StartCoroutine(ChasePlayer());
    }

    public void LostPlayer()
    {
        StartCoroutine(WalkingToPoints());
        light.color = Color.blue;
        eyeMaterial.SetColor("_Emission", Color.blue);
    }

    private IEnumerator WalkingToPoints()
    {
        //var pointIndex = FindNearestPoint(transform.position, navPoints);
        if (navPoints.Length <= 0) throw new ArgumentException("No navigation points");
        while (true)
        {
            var pointIndex = Random.Range(0, navPoints.Length - 1);
            agent.isStopped = false;
            StartWalkAnim();
            agent.destination = navPoints[pointIndex].position;
            yield return new WaitUntil(
                () => Vector3.Distance(agent.destination, transform.position) < agent.stoppingDistance || canSeePlayer);
            agent.isStopped = true;
            StopWalkAnim();
            yield return new WaitForDone(5, () => canSeePlayer);
            if (!canSeePlayer) continue;

            light.color = Color.yellow;
            eyeMaterial.SetColor("_Emission", Color.yellow);
            yield return new WaitForSeconds(.5f);

            if (!canSeePlayer)
            {
                light.color = Color.blue;
                eyeMaterial.SetColor("_Emission", Color.blue);
                continue;
            }

            StartCoroutine(ChasePlayer());
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
        var objectsArea = Physics.OverlapSphere(transform.position, radiusFieldsView, playerLayer);

        if (objectsArea.Length != 0)
        {
            var target = objectsArea[0].transform;

            var directionToTarget = (target.position - transform.position + new Vector3(0,1,0)).normalized;
            if (
                ((Vector3.Angle(transform.forward, directionToTarget) < AngleView / 2 &&
                Physics.Raycast(new Ray(transform.position, directionToTarget), out var hitInfo, radiusFieldsView, viewRaycastLayerMask)) 
                 || Vector3.Distance(transform.position, target.position) < 1) &&
                target.transform.gameObject.layer == 6
                )
            {
                if (canSeePlayer) return;
                canSeePlayer = true;
                chaseTarget = target.transform.parent.gameObject.transform;
            }
            else
            {
                canSeePlayer = false;
                chaseTarget = null;
            }
        }
        else if (canSeePlayer)
        {
            chaseTarget = null;
            canSeePlayer = false;
        }
    }

    public void CheckingAttackCondition()
    {
        if (!canSeePlayer) return;
        if (canShot && Vector3.Distance(transform.position, chaseTarget.position) < distanceAttack)
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

    private void StartWalkAnim()
    {
        StopCoroutine(nameof(AnimChanger));
        StartCoroutine(AnimChanger(1));
    }

    private void StopWalkAnim()
    {
        StopCoroutine(nameof(AnimChanger));
        StartCoroutine(AnimChanger(0));
    }

    private IEnumerator AnimChanger(float state)
    {
        var animatorState = animator.GetFloat("motion");
        var off = (animatorState - state) / 10;
        while (Math.Abs(state - animatorState) >= 0.1)
        {
            animatorState -= off;
            animator.SetFloat("motion", animatorState);
            yield return new WaitForSeconds(0.05f);
        }
        animator.SetFloat("motion", state);
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