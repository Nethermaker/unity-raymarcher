# Unity Raymarcher
A simple raymarcher created in Unity using a compute shader.

Developed and tested on version 2020.2.6. Probably works on newer versions as well, but no guarantees.

The actual raymarching code is inside the compute shader itself (`Assets/Shaders/RayMarchingShader`). It's paired with a helper MonoBehavior script which sets the inputs for the shader and dispatches it, as well as allows it to run in the scene view, as well as the game view.

## Features
* Basic ray marching.
* Basic lighting, using normals estimated from the SDF.
* Slightly buggy soft shadows.

## Known Issues
* The functionality for rendering the actual Unity scene in addition to the raymarched scene isn't working correctly. Has something to do with how I'm getting the distance from the depth buffer.

## Possible Future Improvements
* Add shape/SDF inputs to the compute shader, which would allow creating and transforming objects from within the scene editor instead of only through code.

## Example
![](https://cdn.discordapp.com/attachments/281249883263074305/1068021441326497862/image.png)