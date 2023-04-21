using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManiacMovementController : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    private float airMultiplier;
    private bool readyToJump;

    public float playerHeight;
    public LayerMask groundLayer;
    private bool isOnGround;

    public Transform orientation;

    private float horizontalInput;
    private float verticalInput;
    private bool jumpInput;
    private float groundDrag;
    private Vector3 moveDirection;
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
        isOnGround = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, groundLayer);
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
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        else if (!isOnGround)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        if (jumpInput && isOnGround)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void ResetJump()
    {
        readyToJump = true;
    }
}