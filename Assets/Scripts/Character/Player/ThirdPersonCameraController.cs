using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class ThirdPersonCameraController : MonoBehaviourPunCallbacks, IPunObservable
{
    [Header("References")] public CinemachineFreeLook cinemachineVirtualCamera;
    public PhotonView view;
    public Transform orientation;
    public Transform player;
    public Transform playerModel;

    [Header("Sensitivity Parameters")] public float rotationModelSpeed;
    public float maxXSensitivity;
    public float maxYSensitivity;

    [SerializeField]
    private ScriptableRendererFeature rendererFeature;

    private Slider sensitivitySlider;

    private Vector3 targetPosition;
    private Quaternion targetRotation;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(playerModel.rotation);
            stream.SendNext(playerModel.position);
        }
        else
        {
            playerModel.rotation = (Quaternion)stream.ReceiveNext();
            playerModel.position = (Vector3)stream.ReceiveNext();
        }
    }

    private void Awake()
    {
        if (!view.IsMine)
        {
            cinemachineVirtualCamera.enabled = false;
        }
        targetPosition = transform.position;
        targetRotation = transform.rotation;
    }

    private void Start()
    {
        rendererFeature.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        if (view.IsMine)
        {
            cinemachineVirtualCamera = gameObject.GetComponent<CinemachineFreeLook>();
            var parent = gameObject.transform.parent.transform;
            cinemachineVirtualCamera.Follow = parent;
            cinemachineVirtualCamera.LookAt = parent;
            ChangeSensitivity(GameSettingSaver.settings.Sensitivity * 100);
        }
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        if (view.IsMine)
        {
            Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
            orientation.forward = viewDir.normalized;
            Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;
            if (inputDir != Vector3.zero)
            {
                playerModel.forward = Vector3.Slerp(playerModel.forward, inputDir.normalized,
                    Time.deltaTime * rotationModelSpeed);
            }
            targetPosition += viewDir * rotationModelSpeed * Time.fixedDeltaTime;
            targetRotation *= Quaternion.Euler(inputDir * rotationModelSpeed * Time.fixedDeltaTime);
        }
    }

    public void ChangeSensitivity(float sensitivity)
    {
        sensitivity *= 0.01f;
        GameSettingSaver.settings.Sensitivity = sensitivity;
        cinemachineVirtualCamera.m_XAxis.m_MaxSpeed = maxXSensitivity * sensitivity;
        cinemachineVirtualCamera.m_YAxis.m_MaxSpeed = maxYSensitivity * sensitivity;
    }

    public void SetSensitivitySlider(Slider sensitivitySlider)
    {
        this.sensitivitySlider = sensitivitySlider;
        this.sensitivitySlider.onValueChanged.AddListener(ChangeSensitivity);
    }
}