// @author : xue
// @created : 2022,11,11,8:43
// @desc:

using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace DefaultNamespace
{
    [ExecuteAlways]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Camera))]
    public class CameraAdditionRenderDepth : MonoBehaviour
    {
        private static CameraEvent s_cameraEvent = CameraEvent.AfterDepthTexture;
        
        private CommandBuffer m_commandBuffer;

        private Material m_depthOnlyMaterial;

        private bool _isCameraEnableDepthMode()
        {
            return (GetComponent<Camera>().depthTextureMode & DepthTextureMode.Depth) > 0;
        }

        public virtual Shader depthOnlyShader
        {
            get
            {
                return Shader.Find("DepthOnly");
            }
        }

        private CommandBuffer commandBuffer
        {
            get
            {
                if (m_commandBuffer == null)
                {
                    m_commandBuffer = new CommandBuffer();
                    m_commandBuffer.name = nameof(CameraAdditionRenderDepth);
                }

                return m_commandBuffer;
            }
        }

        private void Awake()
        {
            // init m_depthOnlyMaterial
            Shader local_depthOnlyShader = depthOnlyShader;
            if (local_depthOnlyShader != null)
            {
                m_depthOnlyMaterial = new Material(local_depthOnlyShader);
                m_depthOnlyMaterial.hideFlags = HideFlags.HideAndDontSave;
            }
            else
            {
                Debug.LogError($"[CameraAdditionRenderDepth] depthOnlyShader, can not find");
            }
        }

        private void OnPreCull()
        {
            commandBuffer.Clear();
        }

        private void OnEnable()
        {
            GetComponent<Camera>().AddCommandBuffer(s_cameraEvent, commandBuffer);
        }

        private void OnDisable()
        {
            GetComponent<Camera>().RemoveCommandBuffer(s_cameraEvent, commandBuffer);
        }

        private bool _canAddRenderDepth()
        {
            if (!_isCameraEnableDepthMode())
                return false;

            if (m_depthOnlyMaterial == null)
                return false;

            if (!enabled)
                return false;

            return true;
        }
        
        public void AddRenderDepth(Renderer renderer)
        {
            if (!_canAddRenderDepth())
                return;
            
            commandBuffer.Clear();
            commandBuffer.DrawRenderer(renderer, m_depthOnlyMaterial, 0);
        }
        
        public void AddRenderDepth(Renderer renderer, int submeshIndex)
        {
            if (!_canAddRenderDepth())
                return;
            
            commandBuffer.Clear();
            commandBuffer.DrawRenderer(renderer, m_depthOnlyMaterial, submeshIndex, 0);
        }
        
        private void OnDestroy()
        {
            if (m_depthOnlyMaterial != null)
            {
#if UNITY_EDITOR
                if (!Application.isPlaying)
                {
                    DestroyImmediate(m_depthOnlyMaterial);
                }
                else
                {
                    Destroy(m_depthOnlyMaterial);
                }
#else
                Destroy(m_depthOnlyMaterial);
#endif
                m_depthOnlyMaterial = null;
            }
        }
    }
}