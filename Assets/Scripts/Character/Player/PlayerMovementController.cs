using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UIElements;
using Unity.VisualScripting;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovementController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float jumpForce;
    public float gravityForce;
    public float resistanceForce;



    [Header("Character Parameters")]
    public string nameModelWithAnimator;

    public Transform orientation;

    public bool canMove = true;
    private float horizontalInput;
    private float verticalInput;

    private Vector3 moveDirection;

    [SerializeField] private Animator animator;
    private CharacterController cc;
    private float verticalForce;
    private Vector3 addForce;

    private void Start()
    {
        cc = GetComponent<CharacterController>();
        //animator = GetComponentInChildren<Transform>().Find(nameModelWithAnimator).GetComponent<Animator>();
    }

    private void Update()
    {
        MyInput();
        GravityHandling();
        ForceHandling();
        JumpLogic();
        MovePlayer();
        if(!canMove) return;
        animator.SetFloat("FrontMove", OnversionRange(new Vector2(cc.velocity.x, cc.velocity.z).magnitude, moveSpeed));
    }

    private void ForceHandling()
    {
        if (addForce.magnitude == 0) return;

        switch (addForce.x)
        {
            case > 0:
            {
                addForce.x -= resistanceForce;
                if (addForce.x < 0) addForce.x = 0;
                break;
            }
            case < 0:
            {
                addForce.x += resistanceForce;
                if (addForce.x > 0) addForce.x = 0;
                break;
            }
        }

        switch (addForce.z)
        {
            case > 0:
            {
                addForce.z -= resistanceForce;
                if (addForce.z < 0) addForce.z = 0;
                break;
            }
            case < 0:
            {
                addForce.z += resistanceForce;
                if (addForce.z > 0) addForce.z = 0;
                break;
            }
        }

    }

    private void GravityHandling()
    {
        if (!cc.isGrounded)
        {
            verticalForce -= gravityForce * Time.deltaTime;
        }
        else
        {
            verticalForce = 0;
        }
    }

    public void AddForce(Vector3 force)
    {
        verticalForce += force.y;
        force.y = 0;
        addForce += force;
    }


    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer()
    {
        moveDirection = Vector3.zero;
        if (canMove)
            moveDirection = (orientation.forward * verticalInput + orientation.right * horizontalInput).normalized;
        moveDirection.y =  verticalForce;
        moveDirection += addForce;
        cc.Move(moveSpeed * Time.deltaTime * moveDirection);
    }
    private void JumpLogic()
    {
        if (canMove && cc.isGrounded && Input.GetKey(KeyCode.Space))
        {
            verticalForce += jumpForce;
            animator.SetTrigger("Jumping");
            
        }
        //StartCoroutine(Jump());
    }

    private IEnumerator Jump()
    {
        //canJump = false;
        animator.SetTrigger("Jumping");
        yield return new WaitForSeconds(0.3f);
        verticalForce += jumpForce;
        //yield break;
        yield return new WaitForSeconds(0.5f);
        //canJump = true;
    }

    private float OnversionRange(float valueConverted, float inputRangeMax,
        float outputRangeMax = 1, float inputRangeMin = 0, float outputRangeMin = 0)
    {
        var diffOutputRange = MathF.Abs(outputRangeMax - outputRangeMin);
        var diffInputRange = MathF.Abs(inputRangeMax - inputRangeMin);
        var convFactor = (diffOutputRange / diffInputRange);
        return (outputRangeMin + (convFactor * (valueConverted - inputRangeMin)));
    }
}