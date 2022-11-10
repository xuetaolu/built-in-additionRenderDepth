using System;
using UnityEngine;

namespace DefaultNamespace
{
    [ExecuteAlways]
    [RequireComponent(typeof(Camera))]
    public class CameraRenderDepth : UnityEngine.MonoBehaviour
    {
        private void OnEnable()
        {
            GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
        }

        private void OnDisable()
        {
            GetComponent<Camera>().depthTextureMode = DepthTextureMode.None;
        }
    }
}