using UnityEngine;
using UnityEngine.Events;

namespace InfimaGames.LowPolyShooterPack.Interface
{
    public class CanvasSpawner : MonoBehaviour
    {
        [SerializeField] private UnityEvent<GameObject> _canvasInstantiated;
        [SerializeField] private UnityEvent<GameObject> _canvasPauseInstantiated;
        [SerializeField] private UnityEvent<GameObject> _canvasDeathInstantiated;
        [SerializeField] private UnityEvent<GameObject> _canvasCompleteInstantiated;
        [SerializeField] private UnityEvent<GameObject> _canvasTradeInstantiated;
        [SerializeField] private UnityEvent<GameObject> _canvasChoseInstantiated;

        public GameObject Canvas { get; private set; }
        public GameObject Pause { get; private set; }
        public GameObject Death { get; private set; }
        public GameObject Complete { get; private set; }
        public GameObject Trade { get; private set; }
        public GameObject Chosen { get; private set; }

        [SerializeField] private GameObject canvasPrefab;
        [SerializeField] private GameObject qualitySettingsPrefab;
        [SerializeField] private GameObject _deathPrefab;
        [SerializeField] private GameObject _completePrefab;
        [SerializeField] private GameObject _tradePrefab;
        [SerializeField] private GameObject _chosePrefab;

        private void Start()
        {
            Canvas = Instantiate(canvasPrefab);
            Pause = Instantiate(qualitySettingsPrefab);
            Death = Instantiate(_deathPrefab);
            Complete = Instantiate(_completePrefab);
            Trade = Instantiate(_tradePrefab);
            Chosen = Instantiate(_chosePrefab);

            _canvasInstantiated?.Invoke(Canvas);
            _canvasPauseInstantiated?.Invoke(Pause);
            _canvasDeathInstantiated?.Invoke(Death);
            _canvasCompleteInstantiated?.Invoke(Complete);
            _canvasTradeInstantiated?.Invoke(Trade);
            _canvasChoseInstantiated?.Invoke(Chosen);
        }
    }
}