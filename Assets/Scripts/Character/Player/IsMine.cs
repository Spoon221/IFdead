using UnityEngine;
using Photon.Pun;

public class IsMine : MonoBehaviourPunCallbacks
{
    public PlayerMovementController scriptPlayerMovementController;
    public ManiacMovementController scriptManiacMovementController;
    public Canvas Bar;
    public Camera camera;
    public AudioListener audioListener;
    public RectTransform GeneratorRect;

    private void Awake()
    {
        if (!photonView.IsMine)
        {
            audioListener.enabled = false;
            camera.enabled = false;
            Bar.enabled = false;
            scriptPlayerMovementController.enabled = false;
            scriptManiacMovementController.enabled = false;
        }
    }
}
