using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FisrtPersonCameraController : MonoBehaviour, IPunObservable
{
    public float sensX;
    public float sensY;

    public Transform orientation;
    public Transform maniacModel;
    private float xRotation;
    private float yRotation;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //ќдно из этого возможно лишнее (все работает, но не отрицаю этот факт). –азобратьс€ после выкота спавна
        if (stream.IsWriting)
        {
            stream.SendNext(transform.rotation);
            stream.SendNext(maniacModel.rotation);
        }
        else
        {
            transform.rotation = (Quaternion)stream.ReceiveNext();
            maniacModel.rotation = (Quaternion)stream.ReceiveNext();
        }
    }

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