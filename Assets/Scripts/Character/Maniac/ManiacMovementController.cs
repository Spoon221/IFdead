using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManiacMovementController : MonoBehaviour
{
    [Header("Movement Parameters")] 
    public float MoveSpeed;
    public float JumpForce;
    public float ManiacHeight;

    [Header("Maniac Parameters")] 
    public LayerMask GroundLayer;
    public PhotonView view;
    public Transform orientation;

    private float airMultiplier;
    private bool readyToJump;
    private bool isOnGround;

    private float horizontalInput;
    private float verticalInput;
    private bool jumpInput;
    private Vector3 moveDirection;

    private float groundDrag;
    private Rigidbody rb;


    private void Start()
    {
        groundDrag = 5;
        airMultiplier = 0.4f;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
    }

    private void Update()
    {
        isOnGround = Physics.Raycast(transform.position, Vector3.down, ManiacHeight * 0.5f + 0.3f, GroundLayer);
        MyInput();
        SpeedControl();

        if (isOnGround)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    private void FixedUpdate()
    {
        MovePlayer();
        Jump();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        jumpInput = Input.GetKey(KeyCode.Space);
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        if (isOnGround)
            rb.AddForce(moveDirection.normalized * MoveSpeed * 10f, ForceMode.Force);
        else if (!isOnGround)
            rb.AddForce(moveDirection.normalized * MoveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > MoveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * MoveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        if (jumpInput && isOnGround)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(transform.up * JumpForce, ForceMode.Impulse);
        }
    }
}