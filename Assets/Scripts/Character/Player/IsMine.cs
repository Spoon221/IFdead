using UnityEngine;
using Photon.Pun;

public class IsMine : MonoBehaviour
{
    public PhotonView view;
    public PlayerMovementController scriptPlayerMovementController;
    public ManiacMovementController scriptManiacMovementController;
    public Canvas Bar;
    public Camera camera;

    private void Awake()
    {
        view = GetComponent<PhotonView>();
        if (!view.IsMine)
        {
            camera.enabled = false;
            Bar.enabled = false;
            scriptPlayerMovementController.enabled = false;
            scriptManiacMovementController.enabled = false;
        }
    }
}
