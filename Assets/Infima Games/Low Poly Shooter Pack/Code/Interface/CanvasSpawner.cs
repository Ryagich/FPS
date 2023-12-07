//Copyright 2022, Infima Games. All Rights Reserved.

using UnityEngine;
using UnityEngine.Events;

namespace InfimaGames.LowPolyShooterPack.Interface
{
    /// <summary>
    /// Player Interface.
    /// </summary>
    public class CanvasSpawner : MonoBehaviour
    {
        [SerializeField] private UnityEvent<GameObject> _canvasInstantiated;
        [SerializeField] private UnityEvent<GameObject> _canvasPauseInstantiated;
        
        #region FIELDS SERIALIZED
        [Title(label: "Settings")]
        [Tooltip("Canvas prefab spawned at start. Displays the player's user interface.")]
        [SerializeField]
        private GameObject canvasPrefab;

        [Tooltip(
            "Quality settings menu prefab spawned at start. Used for switching between different quality settings in-game.")]
        [SerializeField]
        private GameObject qualitySettingsPrefab;

        #endregion

        #region UNITY
        private void Awake()
        {
            var canvas = Instantiate(canvasPrefab);
            var pause = Instantiate(qualitySettingsPrefab);
            _canvasInstantiated?.Invoke(canvas);
            _canvasPauseInstantiated.Invoke(pause);
        }
        #endregion
    }
}