using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Man : MonoBehaviour
{
    private NavMeshAgent agent;
    public LayerMask layerMask;
    public Transform surv;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        agent.SetDestination(surv.position);
    }

    private bool CanSeeSurv()
    {
        if (Vector3.Distance(surv.position, transform.position) > 20) return false;

        Ray ray = new(transform.position + new Vector3(0, 2, 0), surv.position - new Vector3(0, 1, 0) - transform.position);
        Debug.DrawRay(transform.position + new Vector3(0, 2, 0), surv.position - new Vector3(0, 1, 0) - transform.position);
        Physics.Raycast(ray, out var hit, layerMask);
        return hit.collider.tag == "Survival";
    }
}
