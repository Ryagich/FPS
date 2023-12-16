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
        [SerializeField] private UnityEvent<GameObject> _canvasDeathInstantiated;
        [SerializeField] private UnityEvent<GameObject> _canvasCompleteInstantiated;

        public GameObject Canvas { get; private set; }
        public GameObject Pause { get; private set; }
        public GameObject Death { get; private set; }
        public GameObject Complete { get; private set; }

        [SerializeField] private GameObject canvasPrefab;
        [SerializeField] private GameObject qualitySettingsPrefab;
        [SerializeField] private GameObject _deathPrefab;
        [SerializeField] private GameObject _completePrefab;

        private void Awake()
        {
            Canvas = Instantiate(canvasPrefab);
            Pause = Instantiate(qualitySettingsPrefab);
            Death = Instantiate(_deathPrefab);
            Complete = Instantiate(_completePrefab);
            
            _canvasInstantiated?.Invoke(Canvas);
            _canvasPauseInstantiated.Invoke(Pause);
            _canvasDeathInstantiated.Invoke(Death);
            _canvasCompleteInstantiated.Invoke(Complete);
        }
    }
}