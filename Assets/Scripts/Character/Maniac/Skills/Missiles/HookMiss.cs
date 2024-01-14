using System.Collections;
using Photon.Pun;
using UnityEngine;

public class HookMiss : MonoBehaviourPun
{
    public float speed;
    public float maxDistance;
    private LineRenderer lineRenderer;
    public ManiacHook parentManiac;
    private Vector3 direction;

    private bool hooked;
    private Transform hookedPlayer;
    private PlayerMovementController playerController;
    public GameObject LeftHand;

    public bool Hooked => hooked;


    private void Start()
    {
        transform.parent = null;
        lineRenderer = GetComponent<LineRenderer>();
        LeftHand = GameObject.FindWithTag("ManiacLeftHand");
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (!Hooked)
        {
            lineRenderer.SetPosition(1, transform.position);
            transform.position += direction.normalized * Time.deltaTime * speed;
        }
        else
        {
            lineRenderer.SetPosition(1, hookedPlayer.position);
        }
        lineRenderer.SetPosition(0, parentManiac.transform.position);

        if (Vector3.Distance(parentManiac.transform.position, transform.position) > maxDistance)
        {
            photonView.RPC("ReturnBackRPC", RpcTarget.All);
        }
    }

    public void Launch(Vector3 direction)
    {
        transform.position = parentManiac.transform.position;
        this.direction = direction;
        gameObject.SetActive(true);
        LeftHand.SetActive(false);
    }

    //private void OnTriggerEnter(Collider collider)
    //{
    //    if(parentManiac.gameObject.GetPhotonView().IsMine)
    //        photonView.RPC("GetColliderRPC", RpcTarget.All, collider.transform.position);
    //}

    //[PunRPC]
    //private void GetColliderRPC(Vector3 pos)
    //{
    //    Physics.SphereCast(pos, GetComponent<SphereCollider>().radius + 0.1f, Vector3.zero, out var hit);

    //    var hitCollider = hit.collider;
    //    CalculateMoves(hitCollider);
    //}

    private void OnTriggerEnter(Collider collider)
    {
        if (!parentManiac.gameObject.GetPhotonView().IsMine) return;
        if (Hooked) return;
        if (collider.gameObject.layer != 6)
        {
            photonView.RPC("ReturnBackRPC", RpcTarget.All);
            //ReturnBackRPC();
            return;
        }

        if (!collider.TryGetComponent(out playerController)) return;
        StopAllCoroutines();
        
        photonView.RPC("SetHookedPlayer", RpcTarget.All, collider.gameObject.GetPhotonView().ViewID);
        hooked = true;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        photonView.RPC("AttractRPC", RpcTarget.MasterClient);
    }

    [PunRPC]
    private void AttractRPC()
    {
        StartCoroutine(AttractPlayer());
    }

    [PunRPC]
    private void SetHookedPlayer(int viewID)
    {
        hookedPlayer = PhotonNetwork.GetPhotonView(viewID).transform;
    }

    [PunRPC]
    private void ReturnBackRPC()
    {
        direction = Vector3.zero;
        StartCoroutine(ReturnBack());
    }


    private IEnumerator AttractPlayer()
    {
        while (Vector3.Distance(parentManiac.transform.position, hookedPlayer.position) >= 1)
        {

            var dir = (parentManiac.transform.position - hookedPlayer.position).normalized;
            hookedPlayer.GetComponent<PlayerMovementController>().AddForce(dir * 7);
            yield return new WaitForFixedUpdate();
        }

        parentManiac.GetComponent<ManiacMinigame>().StartMiniGame(hookedPlayer);

    }

    public void UnHook()
    {
        hookedPlayer = null;
        hooked = false;
        direction = Vector3.zero;
        gameObject.SetActive(true);
        StartCoroutine(ReturnBack());
    }

    private IEnumerator ReturnBack()
    {
        while (Vector3.Distance(parentManiac.transform.position, transform.position) > 1)
        {
            transform.LookAt(parentManiac.transform);
            transform.position += transform.forward * (speed + 1) * Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        gameObject.SetActive(false);
        LeftHand.SetActive(true);
    }
}
