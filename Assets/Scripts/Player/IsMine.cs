using UnityEngine;
using Photon.Pun;

public class IsMine : MonoBehaviour
{
    public PhotonView view;
    public PlayerMovementController scriptPlayerMovementController;
    public ThirdPersonCameraController scriptThirdPersonCameraController;
    public ManiacMovementController scriptManiacMovementController;

    private void Awake()
    {
        view = GetComponent<PhotonView>();
        if (!view.IsMine)
        {
            scriptPlayerMovementController.enabled = false;
            //scriptThirdPersonCameraController.enabled = false;
            scriptManiacMovementController.enabled = false;
        }
    }

}
