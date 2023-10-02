using UnityEngine;
using Photon.Pun;

public class IsMine : MonoBehaviourPun
{
    public PlayerMovementController scriptPlayerMovementController;
    public ManiacMovementController scriptManiacMovementController;
    public ItemManager scriptPlayerItemManager;
    public Canvas Bar;
    public Camera camera;
    public AudioListener audioListener;

    private void Awake()
    {
        if (!photonView.IsMine)
        {
            audioListener.enabled = false;
            camera.enabled = false;
            Bar.enabled = false;
            scriptPlayerMovementController.enabled = false;
            scriptPlayerItemManager.enabled = false;
            scriptManiacMovementController.enabled = false;
        }
    }
}
