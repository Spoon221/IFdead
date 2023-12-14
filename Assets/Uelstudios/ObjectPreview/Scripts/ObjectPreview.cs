using System;
using UnityEngine;

public class ObjectPreview : MonoBehaviour
{
    //The UI Image. (RawImage supports RenderTexture)
    public MeshRenderer meshRenderer;

    //The properties to render the object. (Offset from the cam, rotation and scale)
    public ObjectProperties objectProperties;

    //The resolution of the RenderTexture. (Should match the size of the RawImage(image))
    public int textureResolution = 64;

    //The RenderTexture assigned to the RawImage component.
    private RenderTexture renderTexture;

    [Serializable]
    public class ObjectProperties
    {
        public GameObject gameObjectToPreview;
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale = Vector3.one;
    }

    void Start()
    {
        //Create new RenderTexture.
        renderTexture = new RenderTexture(textureResolution, textureResolution, 16);

        //Assign RenderTexture to Image.
        meshRenderer.material.mainTexture = renderTexture;

        //Render gameObjectToPreview to the RenderTexture.
        Render();
    }

    public void Render()
    {
        //Only render if gameObjectToPreview is != null. (Just to prevent some errors)
        if (objectProperties.gameObjectToPreview != null)
        {
            /*
             * Add a new RenderTask to the ObjectPreviewRenderer.
             * The provided renderTexture will be updated automatically.
             */
            RenderTask renderTask = new RenderTask(renderTexture, objectProperties.gameObjectToPreview,
                objectProperties.position, Quaternion.Euler(objectProperties.rotation), objectProperties.scale);
            ObjectPreviewRenderer.current.RenderPreview(renderTask);
        }
        else
        {
            //Show a warning because gameObjectToPreview is not assigned!
            Debug.LogWarning(string.Format("gameObjectToPreview of ObjectPreview({0}) is not assigned!", name));
        }
    }


    /// <summary>
    /// Sets the gameObjectToPreview in objectProperties.
    /// </summary>
    /// <param name="go">The GameObject to preview</param>
    public void SetObjectToPreview(GameObject go)
    {
        objectProperties.gameObjectToPreview = go;
    }
}
