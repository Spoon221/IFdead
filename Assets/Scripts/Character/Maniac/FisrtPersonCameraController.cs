using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class FisrtPersonCameraController : MonoBehaviour, IPunObservable
{
    public float maxSensX;
    public float maxSensY;
    public Transform orientation;
    public Transform maniacModel;
    public Transform Skill;
    private float currentSensX;
    private float currentSensY;
    private float xRotation;
    private float yRotation;
    private Slider sensitivitySlider;
    [SerializeField]
    private ScriptableRendererFeature rendererFeature;

    public PhotonView view;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(maniacModel.rotation);
            stream.SendNext(Skill.rotation);
            stream.SendNext(Skill.position);
        }
        else
        {
            maniacModel.rotation = (Quaternion) stream.ReceiveNext();
            Skill.rotation = (Quaternion) stream.ReceiveNext();
            Skill.position = (Vector3)stream.ReceiveNext();
        }
    }

    void Start()
    {
        rendererFeature.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        ChangeSensitivity(GameSettingSaver.settings.Sensitivity * 100);
    }

    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * currentSensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * currentSensY;
        if (view.IsMine)
        {
            xRotation = Mathf.Clamp(xRotation - mouseY, -90f, 90f);
            yRotation += mouseX;

            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            orientation.rotation = Quaternion.Euler(0, yRotation, 0);
            maniacModel.rotation = Quaternion.Euler(0, yRotation, 0);
        }
    }
    
    public void ChangeSensitivity(float sensitivity)
    {
        sensitivity *= 0.01f;
        GameSettingSaver.settings.Sensitivity = sensitivity;
        currentSensX = maxSensX * sensitivity;
        currentSensY = maxSensY * sensitivity;
    }
    
    public void SetSensitivitySlider(Slider sensitivitySlider)
    {
        this.sensitivitySlider = sensitivitySlider;
        this.sensitivitySlider.onValueChanged.AddListener(ChangeSensitivity);
    }
}