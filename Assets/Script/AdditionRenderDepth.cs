using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace DefaultNamespace
{
    [ExecuteAlways]
    [RequireComponent(typeof(Renderer))]
    public class AdditionRenderDepth : MonoBehaviour
    {
        private CommandBuffer m_commandBuffer;

        private Dictionary<Camera, bool> m_cameraMarks = new Dictionary<Camera, bool>();

        private static CameraEvent s_cameraEvent = CameraEvent.AfterDepthTexture;

        private void OnEnable()
        {
            
        }

        private void OnDisable()
        {
            if (m_commandBuffer != null)
            {
                foreach (var VARIABLE in m_cameraMarks)
                {
                    var camera = VARIABLE.Key;
                    if (camera != null)
                    {
                        camera.RemoveCommandBuffer(s_cameraEvent, m_commandBuffer);
                    }
                }
            }
    
            m_cameraMarks.Clear();

        }

        private void Update()
        {
            if(m_commandBuffer != null)
                m_commandBuffer.Clear();
        }

        private void OnWillRenderObject()
        {
            // var mesh = GetComponent<SkinnedMeshRenderer>()?.sharedMesh;
            // if (mesh == null)
            // {
            //     mesh = GetComponent<MeshFilter>().sharedMesh;
            // }
            //
            // if (mesh == null)
            // {
            //     return;
            // }

            var renderer = GetComponent<Renderer>();

            var material = renderer.sharedMaterial;

            if (material == null)
                return;

            var camera = Camera.current;

            _initCommandBuffer(renderer, material, camera);
            
            

        }

        private void _initCommandBuffer(Renderer renderer, Material material, Camera camera)
        {
            if (m_commandBuffer == null)
            {
                m_commandBuffer = new CommandBuffer();
                m_commandBuffer.name = nameof(AdditionRenderDepth);
            }

            if (m_cameraMarks.ContainsKey(camera))
            {
                // pass
            }
            else
            {
                camera.AddCommandBuffer(s_cameraEvent, m_commandBuffer);
                m_cameraMarks[camera] = true;
            }
            

            m_commandBuffer.Clear();

            if ((camera.depthTextureMode & DepthTextureMode.Depth) > 0)
            {
                int pass = material.FindPass("ShadowCaster");
                m_commandBuffer.DrawRenderer(renderer, material, 0, pass);
            }


        }
    }
}