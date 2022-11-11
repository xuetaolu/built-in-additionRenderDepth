using System;
using Unity.VisualScripting;
using UnityEngine;

namespace DefaultNamespace
{
    [ExecuteAlways]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Renderer))]
    public class RendererAdditionRenderDepth : MonoBehaviour
    {

        public readonly static int s_transparentStartQueue = 2501;
        
        private void OnEnable()
        {
            
        }
        
        private void OnDisable()
        {
            
        }

        private Mesh _getMesh()
        {
            Mesh mesh;
            var skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
            if (skinnedMeshRenderer == null)
            {
                var meshFilter = GetComponent<MeshFilter>();
                mesh = meshFilter.sharedMesh;
            }
            else
            {
                mesh = skinnedMeshRenderer.sharedMesh;
            }

            return mesh;
        }

        private void OnWillRenderObject()
        {

            var renderer = GetComponent<Renderer>();

            var camera = Camera.current;

            var mesh = _getMesh();

            if (mesh == null || mesh.subMeshCount <= 0)
                return;

            int subMeshCount = mesh.subMeshCount;
            
            var cameraAdditionRenderDepth = camera.GetOrAddComponent<CameraAdditionRenderDepth>();

            if (subMeshCount == 1)
            {
                // 暂时不考虑单submesh多个材质的情况
                var material = renderer.sharedMaterial;
                if (_isMaterialNeedAdditionRenderDepth(material))
                {
                    cameraAdditionRenderDepth.AddRenderDepth(renderer);
                }
            }
            else if (subMeshCount > 1)
            {
                var materials = renderer.sharedMaterials;
                var length = Math.Min(subMeshCount, materials.Length);
                for (int i = 0; i < length; i++)
                {
                    var material = i < materials.Length ? materials[i] : null;

                    if (_isMaterialNeedAdditionRenderDepth(material))
                    {
                        cameraAdditionRenderDepth.AddRenderDepth(renderer, i);
                    }
                }
            }
        }
        

        private static bool _isMaterialNeedAdditionRenderDepth(Material material)
        {
            return material != null && material.renderQueue >= s_transparentStartQueue;
        }
        
        
    }
}