using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineController : MonoBehaviour
{
    private Material outlineMaterial;
    [SerializeField]private float scale;

    void Start()
    {
        outlineMaterial = gameObject.GetComponentInParent<Renderer>().materials[1];
        outlineMaterial.SetFloat("_Scale", 0f);
    }


    private void OnTriggerEnter(Collider other)
    {
        outlineMaterial.SetFloat("_Scale", scale);
    }

    private void OnTriggerExit(Collider other)
    {
        outlineMaterial.SetFloat("_Scale", 0f);
    }
}