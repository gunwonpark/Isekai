using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Test_G : MonoBehaviour
{
    private static Material grayscaleMaterial;

    void OnEnable()
    {
        RenderPipelineManager.endCameraRendering += OnEndCameraRendering;

        // 쉐이더를 사용한 머티리얼 초기화
        if (grayscaleMaterial == null)
        {
            Shader grayscaleShader = Shader.Find("Custom/Grayscale");
            grayscaleMaterial = new Material(grayscaleShader);
        }
    }

    void OnDisable()
    {
        RenderPipelineManager.endCameraRendering -= OnEndCameraRendering;
    }

    void OnEndCameraRendering(ScriptableRenderContext context, Camera camera)
    {
        if (grayscaleMaterial != null)
        {
            // 'EffectAmount' 값 서서히 증가시켜서 흑백으로 변환
            float effectAmount = Mathf.PingPong(Time.time / 2f, 1f);  // 예시로 시간이 지남에 따라 변하는 값

            grayscaleMaterial.SetFloat("_EffectAmount", effectAmount);

            // CommandBuffer로 렌더링된 화면을 처리
            CommandBuffer cmd = CommandBufferPool.Get("ApplyGrayscaleEffect");
            cmd.Blit(null, BuiltinRenderTextureType.CameraTarget, grayscaleMaterial);
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }
}
