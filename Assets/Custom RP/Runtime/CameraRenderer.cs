using UnityEngine;
using UnityEngine.Rendering;

public class CameraRenderer
{
    private const string COMMAND_BUFFER_NAME = "Render Camera";

    private static ShaderTagId unlitShaderTagId = new ShaderTagId("SRPDefaultUnlit");

    private ScriptableRenderContext context;
    private Camera camera;
    private CommandBuffer buffer = new CommandBuffer { name = COMMAND_BUFFER_NAME };
    private CullingResults cullingResults;

    public void Render(ScriptableRenderContext context, Camera camera)
    {
        this.context = context;
        this.camera = camera;

        if (!Cull())
            return;

        Setup();
        DrawVisibleGeometry();
        Submit();
    }

    void Setup()
    {
        // 设置摄像机的参数，对于场景中的所有物体而言，摄像机的参数都是相同的，如 view matrix 和 projection matrix
        context.SetupCameraProperties(camera);
        // 如果在设置摄像机参数前就执行 ClearRenderTarget，Unity 会使用 Hidden/InternalClear shader
        // 绘制一个填充整个屏幕的网格的方法来清屏，在 FrameDebugger 里命令是 Draw GL
        buffer.ClearRenderTarget(true, true, Color.clear);
        buffer.BeginSample(COMMAND_BUFFER_NAME);
        ExecuteBuffer();
    }

    bool Cull()
    {
        if (camera.TryGetCullingParameters(out ScriptableCullingParameters p))
        {
            cullingResults = context.Cull(ref p);
            return true;
        }

        return false;
    }

    void DrawVisibleGeometry()
    {
        var sortingSettings = new SortingSettings(camera) {
            criteria = SortingCriteria.CommonOpaque
        };
        var drawingSettings = new DrawingSettings(unlitShaderTagId, sortingSettings);
        var filteringSettings = new FilteringSettings(RenderQueueRange.opaque);
        context.DrawRenderers(cullingResults, ref drawingSettings, ref filteringSettings);
        
        context.DrawSkybox(camera);

        sortingSettings.criteria = SortingCriteria.CommonTransparent;
        drawingSettings.sortingSettings = sortingSettings;
        filteringSettings.renderQueueRange = RenderQueueRange.transparent;
        context.DrawRenderers(cullingResults, ref drawingSettings, ref filteringSettings);
    }

    void Submit()
    {
        buffer.EndSample(COMMAND_BUFFER_NAME);
        ExecuteBuffer();

        // 提交渲染指令，提交后前面设置的渲染指令才会被执行
        context.Submit();
    }

    void ExecuteBuffer()
    {
        context.ExecuteCommandBuffer(buffer);
        buffer.Clear();
    }
}
