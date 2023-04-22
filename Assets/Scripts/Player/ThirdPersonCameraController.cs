using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class ThirdPersonCameraController : MonoBehaviour
{
    [Header("References")] 
    public CinemachineFreeLook cinemachineVirtualCamera;
    public PhotonView view;
    public Transform orientation;
    public Transform player;
    public Transform playerModel;

    [Header("Sensitivity Parameters")] 
    public float rotationModelSpeed;
    public Slider sensitivitySlider;
    public float maxXSensitivity;
    public float maxYSensitivity;


    private void Awake()
    {
        //view = GetComponent<PhotonView>();
        if (!view.IsMine)
        {
            cinemachineVirtualCamera.enabled = false;
        }
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        if (view.IsMine)
        {
            cinemachineVirtualCamera = gameObject.GetComponent<CinemachineFreeLook>();
            var parent = gameObject.transform.parent.transform;
            cinemachineVirtualCamera.Follow = parent;
            cinemachineVirtualCamera.LookAt = parent;
            sensitivitySlider.onValueChanged.AddListener(ChangeSensitivity);
            ChangeSensitivity(sensitivitySlider.value);
        }
    }

    private void Update()
    {
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;
        if (inputDir != Vector3.zero)
            playerModel.forward =
                Vector3.Slerp(playerModel.forward, inputDir.normalized, Time.deltaTime * rotationModelSpeed);
    }

    private void ChangeSensitivity(float sensitivity)
    {
        sensitivity *= 0.01f;
        cinemachineVirtualCamera.m_XAxis.m_MaxSpeed = maxXSensitivity * sensitivity;
        cinemachineVirtualCamera.m_YAxis.m_MaxSpeed = maxYSensitivity * sensitivity;
    }
}