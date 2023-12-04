using UnityEngine;
using Photon.Pun;

public class IsMine : MonoBehaviourPunCallbacks
{
    public PlayerMovementController scriptPlayerMovementController;
    public ManiacMovementController scriptManiacMovementController;
    public Canvas Bar;
    public Camera camera;
    public AudioListener audioListener;

    private void Awake()
    {
        if (!photonView.IsMine)
        {
            OffScripts();
        }
    }

    private void OffScripts()
    {
        audioListener.enabled = false;
        camera.enabled = false;
        Bar.enabled = false;
        scriptPlayerMovementController.enabled = false;
        scriptManiacMovementController.enabled = false;
    }
}
