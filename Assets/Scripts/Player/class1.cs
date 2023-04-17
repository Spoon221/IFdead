using UnityEngine;
using Photon.Pun;

public class class1 : MonoBehaviour
{
    public PhotonView view;
    public PlayerMovementController scripsPlayerMovementController;

    private void Awake()
    {
        view = GetComponent<PhotonView>();
        if (!view.IsMine)
        {
            scripsPlayerMovementController.enabled = false;
        }
    }

}
