using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CompleteUIHolder : MonoBehaviour
{
    public static CompleteUIHolder Instance;
   
    [field: SerializeField] public Transform Complete { get; private set; }
    [field: SerializeField] public Button ExitToMenuButton { get; private set; }
    [field: SerializeField] public Button RewardedButton { get; private set; }
    [field: SerializeField] public TMP_Text RewardedText { get; private set; }
   
    private void Awake()
    {
        Instance = this;
    }
}
