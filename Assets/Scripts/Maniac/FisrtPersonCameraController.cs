using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FisrtPersonCameraController : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;
    public Transform maniacModel;
    private float xRotation;
    private float yRotation;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        xRotation = Mathf.Clamp(xRotation - mouseY, -90f, 90f);
        yRotation += mouseX;

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        maniacModel.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}