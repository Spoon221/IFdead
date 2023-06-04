using UnityEngine;
using Photon.Pun;

public class IsMine : MonoBehaviour
{
    public PhotonView view;
    public PlayerMovementController scriptPlayerMovementController;
    public ManiacMovementController scriptManiacMovementController;
    public ItemManager scriptPlayerItemManager;
    public Canvas Bar;
    public Canvas eCode;
    public Camera camera;

    private void Awake()
    {
        view = GetComponent<PhotonView>();
        if (!view.IsMine)
        {
            camera.enabled = false;
            Bar.enabled = false;
            eCode.enabled = false;
            scriptPlayerMovementController.enabled = false;
            scriptPlayerItemManager.enabled = false;
            scriptManiacMovementController.enabled = false;
        }
    }
}
