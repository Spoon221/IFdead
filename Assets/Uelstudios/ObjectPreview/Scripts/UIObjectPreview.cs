using System;
using UnityEngine;
using UnityEngine.UI;

public class UIObjectPreview : MonoBehaviour
{
    //The UI Image. (RawImage supports RenderTexture)
    public RawImage image;

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
        image.texture = renderTexture;

        //Render gameObjectToPreview to the RenderTexture.
        Render();
    }

    void Render()
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
            Debug.LogWarning(string.Format("gameObjectToPreview of UIObjectRenderer({0}) is not assigned!", name));
        }
    }
}
