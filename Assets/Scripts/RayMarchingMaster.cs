using UnityEngine;

[ExecuteInEditMode, ImageEffectAllowedInSceneView]
public class RayMarchingMaster : MonoBehaviour
{
    public ComputeShader rayMarching;
    public int maxMarchingSteps = 100;
    public int maxMarchingDistance = 100;

    public Vector3 lightDirection = new Vector3(1, 1, -2);
    
    public bool renderWorld;
    
    private RenderTexture renderTexture;

    private Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
        cam.depthTextureMode |= DepthTextureMode.Depth;
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        SetShaderParameters();
        Render(source, destination);
    }
    
    private void Render(RenderTexture source, RenderTexture destination)
    {
        // Make sure we have a current render target
        InitRenderTexture();
        
        // Set the target and dispatch the compute shader
        rayMarching.SetTexture(0, "Result", renderTexture);
        rayMarching.SetTexture(0, "_Input", source);
        rayMarching.SetTextureFromGlobal(0, "_DepthTexture", "_CameraDepthTexture");
        int threadGroupsX = Mathf.CeilToInt(Screen.width / 8.0f);
        int threadGroupsY = Mathf.CeilToInt(Screen.height / 8.0f);
        rayMarching.Dispatch(0, threadGroupsX, threadGroupsY, 1);
        
        // Blit the result texture to the screen
        Graphics.Blit(renderTexture, destination);
    }
    
    private void InitRenderTexture()
    {
        if (renderTexture == null || renderTexture.width != Screen.width || renderTexture.height != Screen.height)
        {
            // Release render texture if we already have one
            if (renderTexture != null)
                renderTexture.Release();
            // Get a render target for Ray Tracing
            renderTexture = new RenderTexture(Screen.width, Screen.height, 0,
                RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
            renderTexture.enableRandomWrite = true;
            renderTexture.Create();
        }
    }

    private void SetShaderParameters()
    {
        rayMarching.SetMatrix("_CameraToWorld", cam.cameraToWorldMatrix);
        rayMarching.SetMatrix("_CameraInverseProjection", cam.projectionMatrix.inverse);
        rayMarching.SetMatrix("_CameraProjection", cam.cullingMatrix);

        rayMarching.SetVector("_Resolution", new Vector4(cam.pixelWidth, cam.pixelHeight, 0, 0));
        
        rayMarching.SetInt("_MaxMarchingSteps", maxMarchingSteps);
        rayMarching.SetFloat("_MaxMarchingDistance", maxMarchingDistance);
        
        rayMarching.SetVector("_LightDirection", lightDirection);
        
        rayMarching.SetBool("_RenderWorld", renderWorld);
    }
}
