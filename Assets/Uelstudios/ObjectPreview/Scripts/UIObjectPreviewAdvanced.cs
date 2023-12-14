using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIObjectPreviewAdvanced : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //The UI Image. (RawImage supports RenderTexture)
    public RawImage image;

    //The properties to render the object. (Offset from the cam, rotation and scale)
    public ObjectProperties objectProperties;

    //rotationSpeed controls how fast the object will rotate if the mouse is over this component.
    public float rotationSpeed = 2f;

    //The resolution of the RenderTexture. (Should match the size of the RawImage(image))
    public int textureResolution = 64;

    //The current rotation of the object
    private float rotation;
    
    //Is the mouse over this component?
    private bool selected = false;

    //Was the mouse over this component?
    private bool lastSelected = false;

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

    private void Update()
    {
        //Is the mouse over this component?
        if (!selected)
        {
            //Reset rotation
            rotation = 0f;

            //Render the zero-rotation if this component was selected last frame.
            if (lastSelected)
                Render();
        }
        else
        {
            //Add rotation
            rotation += rotationSpeed * Time.unscaledDeltaTime;

            //Render the object
            Render();
        }

        //Update lastSelected
        lastSelected = selected;
    }

    void Render()
    {
        //Only render if gameObjectToPreview is != null. (Just to prevent some errors)
        if (objectProperties.gameObjectToPreview != null)
        {
            //Add the current rotation to the rotation of objectProperties
            Quaternion rot = Quaternion.Euler(objectProperties.rotation);
            rot.eulerAngles += new Vector3(0, rotation, 0);
            
            /*
             * Add a new RenderTask to the ObjectPreviewRenderer.
             * The provided renderTexture will be updated automatically.
             */
            RenderTask renderTask = new RenderTask(renderTexture, objectProperties.gameObjectToPreview,
                objectProperties.position, rot, objectProperties.scale);
            ObjectPreviewRenderer.current.RenderPreview(renderTask);
        }
        else
        {
            //Show a warning because gameObjectToPreview is not assigned!
            Debug.LogWarning(string.Format("gameObjectToPreview of UIObjectRenderer({0}) is not assigned!", name));
        }
    }

    //Called when the mouse enters this component
    public void OnPointerEnter(PointerEventData eventData)
    {
        selected = true;
    }

    //Called when the mouse leaves this component
    public void OnPointerExit(PointerEventData eventData)
    {
        selected = false;
    }
}
