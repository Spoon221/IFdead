using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UIElements;
using Unity.VisualScripting;

public class PlayerMovementController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float jumpForce;
    public float groundDrag;
    public float airMultiplier;

    [Header("Сharacter Parameters")]
    public string nameModelWithAnimator;
    public float playerHeight;

    private float lengthPay;

    public Transform orientation;

    private float horizontalInput;
    private float verticalInput;

    private Vector3 moveDirection;

    private Animator animator;
    private Rigidbody rb;

    private void Start()
    {
        lengthPay = playerHeight / 2 + 0.14f;
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Transform>().Find(nameModelWithAnimator).GetComponent<Animator>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        MyInput();
        JumpLogic();
        SpeedControl();
        if (IsGround(lengthPay))
            rb.drag = groundDrag;
        else
            rb.drag = 0;

        animator.SetFloat("FrontMove", OnversionRange(new Vector2(rb.velocity.x, rb.velocity.z).magnitude, moveSpeed));
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
    }
    private void JumpLogic()
    {
        if (IsGround(lengthPay) && Input.GetKeyDown(KeyCode.Space))
            StartCoroutine(Jump());
    }

    private IEnumerator Jump()
    {
        animator.SetTrigger("Jumping");
        yield return new WaitForSeconds(0.3f);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Force);
        yield break;
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
    private bool IsGround(float lRay)
    {
        return Physics.Raycast(rb.worldCenterOfMass, Vector3.down, lRay);
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