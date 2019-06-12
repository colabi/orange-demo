/* Copyright ©Seth Piezas
 */
using UnityEngine;
using System.Collections;

namespace Application
{
    public class AirProjector : MonoBehaviour
    {
        public static AirProjector shared;
        public CRTMonitor monitor;
        public TouchScreenInput touchoverlay;
        public Renderer[] layers;
        public Material screenMaterial;
        public float kNoise = 0.3F;
        void Start()
        {
            AirProjector.shared = this;
            float val = 1.0F;
            screenMaterial = SystemManager.shared.screenMaterial;
            foreach (Renderer r in layers) {
                r.material = screenMaterial;
                var block = new MaterialPropertyBlock();
                block.SetFloat("_Alpha", val);
                val *= 0.75F;
                r.SetPropertyBlock(block);
            }

        }

        void Update()
        {
            Vector4 noiseVec = new Vector4(Random.Range(0, 0.5F), Random.Range(0, 0.5F), kNoise, 0);
            screenMaterial?.SetVector("_NoiseSelector", noiseVec);
        }
    }


}
 