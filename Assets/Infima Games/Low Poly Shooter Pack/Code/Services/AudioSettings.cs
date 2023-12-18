using UnityEngine;

namespace InfimaGames.LowPolyShooterPack
{
    [System.Serializable]
    public struct AudioSettings
    {
        public float Volume => volume;
        public float SpatialBlend => spatialBlend;

        [Header("Settings")]
        [Tooltip("Volume.")]
        [Range(0.0f, 1.0f)]
        [SerializeField]
        private float volume;

        [Tooltip("Spatial Blend.")]
        [Range(0.0f, 1.0f)]
        [SerializeField]
        private float spatialBlend;

        public AudioSettings(float volume = 1.0f, float spatialBlend = 1.0f)
        {
            this.volume = volume;
            this.spatialBlend = spatialBlend;
        }
    }
}