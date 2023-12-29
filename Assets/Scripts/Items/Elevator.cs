using System;
using System.Collections;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public Animator animator;
    public int delayElevatorToDownSeconds;
    private static readonly int ElevatorUp = Animator.StringToHash("ElevatorUp");
    private bool isMoving;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!isMoving)
        {
            isMoving = true;
            StartCoroutine(nameof(OnElevatorUp));
        }
    }

    private IEnumerator OnElevatorUp()
    {
        Debug.Log("лифт должен подниматься");
        animator.SetTrigger(ElevatorUp);

        yield return new WaitForSeconds(9);
        Debug.Log("лифт должен опускаться");
        // animator.SetTrigger(ElevatorDown);
        isMoving = false;
    }
}