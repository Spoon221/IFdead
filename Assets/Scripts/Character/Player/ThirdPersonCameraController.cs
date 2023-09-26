using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;
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

    private Slider sensitivitySlider;

    float currentTime = 0;
    double currentPacketTime = 0;
    double lastPacketTime = 0;
    Vector3 positionAtLastPacket = Vector3.zero;
    Quaternion rotationAtLastPacket = Quaternion.identity;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(playerModel.position);
            stream.SendNext(playerModel.rotation);
        }
        else
        {
            playerModel.position = (Vector3)stream.ReceiveNext();
            playerModel.rotation = (Quaternion)stream.ReceiveNext();
            //orientation.position = (Vector3)stream.ReceiveNext();
            //orientation.rotation = (Quaternion)stream.ReceiveNext();

            currentTime = 0.0f;
            lastPacketTime = currentPacketTime;
            currentPacketTime = info.SentServerTime;
            positionAtLastPacket = playerModel.position;
            rotationAtLastPacket = playerModel.rotation;
        }
    }

    private void Awake()
    {
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
            ChangeSensitivity(50);
        }
    }

    private void Update()
    {
        if (!photonView.IsMine)
        {
            double timeToReachGoal = currentPacketTime - lastPacketTime;
            currentTime += Time.deltaTime;

            transform.position = Vector3.Lerp(positionAtLastPacket, orientation.position, (float)(currentTime / timeToReachGoal));
            transform.rotation = Quaternion.Lerp(rotationAtLastPacket, orientation.rotation, (float)(currentTime / timeToReachGoal));
        }
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        if (view.IsMine)
        {
            Vector3 viewDir = player.position -
                              new Vector3(transform.position.x, player.position.y, transform.position.z);
            orientation.forward = viewDir.normalized;
            Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;
            if (inputDir != Vector3.zero)
            {
                playerModel.forward = Vector3.Slerp(playerModel.forward, inputDir.normalized,
                    Time.deltaTime * rotationModelSpeed);
            }
        }
    }



    private void ChangeSensitivity(float sensitivity)
    {
        sensitivity *= 0.01f;
        cinemachineVirtualCamera.m_XAxis.m_MaxSpeed = maxXSensitivity * sensitivity;
        cinemachineVirtualCamera.m_YAxis.m_MaxSpeed = maxYSensitivity * sensitivity;
    }

    public void SetSensitivitySlider(Slider sensitivitySlider)
    {
        this.sensitivitySlider = sensitivitySlider;
        this.sensitivitySlider.onValueChanged.AddListener(ChangeSensitivity);
    }
}