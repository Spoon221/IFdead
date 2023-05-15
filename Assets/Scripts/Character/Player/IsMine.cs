using UnityEngine;
using Photon.Pun;

public class IsMine : MonoBehaviour
{
    public PhotonView view;
    public PlayerMovementController scriptPlayerMovementController;
    public PlayerStats scriptPlayerStats;
    public ManiacMovementController scriptManiacMovementController;
    public Camera camera;

    private void Awake()
    {
        view = GetComponent<PhotonView>();
        if (!view.IsMine)
        {
            camera.enabled = false;
            scriptPlayerMovementController.enabled = false;
            scriptPlayerStats.enabled = false;
            scriptManiacMovementController.enabled = false;
        }
    }

}
