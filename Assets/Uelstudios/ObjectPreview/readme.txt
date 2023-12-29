Thank you for choosing this asset!

#SETUP# << VERY IMPORTANT
	To get this asset working properly you have to follow these three simple steps:
		1. Create a new layer called "PreviewObject".
		2. Ensure that the layer of the prefab
			"ObjectPreviewRenderer" and all of its children is "PreviewObject"
		3. Drop the prefab in your scene. Done!

#Render to UI#
	1. Create a RawImage within a Canvas.
	2. Add the UIObjectPreview component to the RawImage.
	3. Assign all variables of UIObjectPreview. Done!

#Render to Texture#
	1. Create a GameObject with a MeshRenderer
	2. Add the ObjectPreview component.
	3. Assign all variables of ObjectPreview. Done!
	
#Script examples#
	*Render an object*
	[script]
	RenderTask renderTask = new RenderTask(renderTexture, gameObjectToPreview);
	ObjectPreviewRenderer.current.RenderPreview(renderTask);
	[/script]
	

Look at the AssetStore-Page for additional tutorials.
	
	Best regards Paul.