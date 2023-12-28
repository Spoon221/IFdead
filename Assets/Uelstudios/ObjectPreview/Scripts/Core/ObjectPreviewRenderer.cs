using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectPreviewRenderer : MonoBehaviour
{
    public static ObjectPreviewRenderer current;

    [Header("The camera to render the gameObjects.")]
    public Camera cameraPreview;

    [Header("The transform which holds the previewed gameObjects.")]
    public Transform objectHolder;

    private Queue<RenderTask> renderTasks;

    void Awake()
    {
        current = this;
        renderTasks = new Queue<RenderTask>();
    }

    private void Update()
    {
        //Cycle trough all the RenderTasks (One GameObject per frame)
        if (renderTasks.Count > 0)
        {
            Render(renderTasks.Dequeue());
        }
    }

    /// <summary>
    /// Enquenes the task to be rendered.
    /// Don't use the provided RenderTexture instantly because this function needs some time to render the gameObject.
    /// </summary>
    /// <param name="task">The RenderTask.</param>
    public void RenderPreview(RenderTask task)
    {
        renderTasks.Enqueue(task);
    }

    /// <summary>
    /// Render the GameObject from the task to the target RenderTexture.
    /// </summary>
    /// <param name="task">The RenderTask.</param>
    private void Render(RenderTask task)
    {
        //Remove old objects
        var children = new List<GameObject>();
        foreach (Transform child in objectHolder) children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));

        //Create Object and update transform
        GameObject go = Instantiate(task.gameObject, objectHolder, false);
        go.transform.localScale = task.scale;
        go.transform.localPosition = task.position;
        go.transform.localRotation = task.rotation;

        //Set Layer
        SetLayerRecursively(go, gameObject.layer);

        //Render
        cameraPreview.enabled = true;
        cameraPreview.targetTexture = task.texture;
        cameraPreview.Render();
        cameraPreview.enabled = false;

        //Remove Object
        Destroy(go);
    }

    //Helper function to set layer of all child objects.
    void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (null == obj)
        {
            return;
        }

        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            if (null == child)
            {
                continue;
            }
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }
}
