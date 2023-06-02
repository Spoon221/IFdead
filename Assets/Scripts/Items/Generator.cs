using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Generator : ActivatedItem
{
    private List<GameObject> lights;

    public override void Start()
    {
        base.Start();
        var lampposts = GameObject.FindGameObjectsWithTag("Lamppost").ToList();
        lights = new List<GameObject>();
        foreach (var lamppost in lampposts)
        {
            foreach (var lightPoint in lamppost.gameObject.GetComponentsInChildren<Light>())
            {
                lightPoint.gameObject.SetActive(false);
                lights.Add(lightPoint.gameObject);
            }
        }
    }

    public override void ActivateItem()
    {
        base.ActivateItem();
        foreach (var light in lights)
            light.SetActive(true);
    }
}