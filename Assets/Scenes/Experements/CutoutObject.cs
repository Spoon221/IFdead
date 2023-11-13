using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutoutObject : MonoBehaviour
{
    [SerializeField]
    private Transform targetObject;

    [SerializeField]
    private LayerMask wallMask;

    private const float CutoutSize = 0.4f;

    private Camera mainCamera;

    private List<Material> activeMaterials = new();

    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
    }

    private void Update()
    {
        Vector2 cutoutPos = mainCamera.WorldToViewportPoint(targetObject.position);
        cutoutPos.y /= (Screen.width / Screen.height);

        Vector3 offset = targetObject.position - transform.position;
        RaycastHit[] hitObjects = Physics.RaycastAll(transform.position, offset, offset.magnitude, wallMask);

        for (int i = 0; i < hitObjects.Length; ++i)
        {
            Material[] materials = hitObjects[i].transform.GetComponent<Renderer>().materials;

            for (int m = 0; m < materials.Length; ++m)
            {
                materials[m].SetVector("_CutoutPos", cutoutPos);
                //materials[m].SetFloat("_FalloffSize", 0.05f);
                materials[m].SetFloat("_CutoutSize", CutoutSize);
                activeMaterials.Add(materials[m]);
            }
        }

        StartCoroutine(EndFrame());
    }
    private IEnumerator EndFrame()
    {
        yield return new WaitForEndOfFrame();
        foreach (var activeMaterial in activeMaterials)
        {
            activeMaterial.SetFloat("_CutoutSize", 0);
        }

        activeMaterials = new List<Material>();
    }
}