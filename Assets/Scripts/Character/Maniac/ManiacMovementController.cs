using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ManiacMovementController : MonoBehaviour
{
    [Header("Movement Parameters")]
    public float moveSpeed;
    public float jumpForce;
    public float gravityForce;

    [Header("Maniac Parameters")] 
    public Transform orientation;

    private float horizontalInput;
    private float verticalInput;
    private bool jumpInput;
    private Vector3 moveDirection;
    private float verticalForce;

    //private float groundDrag;
    private CharacterController cc;


    private void Start()
    {
        cc = GetComponent<CharacterController>();

    }

    private void Update()
    {
        MyInput(); 
        GravityHandling();
        JumpLogic();
        MovePlayer();
    }

    private void FixedUpdate()
    {
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        jumpInput = Input.GetKey(KeyCode.Space);
        //jumpInput = Input.GetKey("Jump");
    }

    private void MovePlayer()
    {
        //moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        //if (isOnGround)
        //    rb.AddForce(moveDirection.normalized * MoveSpeed * 10f, ForceMode.Force);
        //else if (!isOnGround)
        //    rb.AddForce(moveDirection.normalized * MoveSpeed * 10f * airMultiplier, ForceMode.Force);

        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        moveDirection.y = verticalForce;
        cc.Move(moveSpeed * moveDirection * Time.deltaTime);
    }

    //private void Jump()
    //{
    //    if (jumpInput && isOnGround)
    //    {
    //        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
    //        rb.AddForce(transform.up * JumpForce, ForceMode.Impulse);
    //    }
    //}

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

    private void JumpLogic()
    {
        if (cc.isGrounded && Input.GetKey(KeyCode.Space))
        {
            verticalForce += jumpForce;
            //animator.SetTrigger("Jumping");
        }
        //StartCoroutine(Jump());
    }
}