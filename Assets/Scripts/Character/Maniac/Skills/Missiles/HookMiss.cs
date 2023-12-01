using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Animations;

public class HookMiss : MonoBehaviour
{
    public float speed;
    public float maxDistance;
    private LineRenderer lineRenderer;
    private ManiacHook parentManiac;
    private Vector3 direction;

    private bool hooked;
    private Transform hookedPlayer;
    private PlayerMovementController playerController;

    public bool Hooked => hooked;


    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
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
            direction = Vector3.zero;
            StartCoroutine(ReturnBack());
        }
    }

    public void Launch(ManiacHook hook, Vector3 direction)
    {
        transform.position = hook.transform.position;
        parentManiac = hook;
        this.direction = direction;
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (Hooked) return;
        if (collider.gameObject.layer != 6)
        {
            direction = Vector3.zero;
            StartCoroutine(ReturnBack());
            return;
        }

        if (!collider.TryGetComponent(out playerController)) return;
        StopAllCoroutines();
        hookedPlayer = collider.transform;
        hooked = true;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        StartCoroutine(AttractPlayer());
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
    }
}
