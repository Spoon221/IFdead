using UnityEngine;
using Photon.Pun;

public class IsMine : MonoBehaviour
{
    public PhotonView view;
    public PlayerMovementController scriptPlayerMovementController;
    public ManiacMovementController scriptManiacMovementController;
    public Canvas Bar;
    public ManaBar scriptManaBar;
    public HealthBar scriptHealthBar;
    public Camera camera;

    private void Awake()
    {
        view = GetComponent<PhotonView>();
        if (!view.IsMine)
        {
            camera.enabled = false;
            Bar.enabled = false;
            scriptManaBar.enabled = false;
            scriptHealthBar.enabled = false;
            scriptPlayerMovementController.enabled = false;
            scriptManiacMovementController.enabled = false;
        }
    }
}
