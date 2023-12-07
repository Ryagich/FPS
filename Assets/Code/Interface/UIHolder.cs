using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHolder : MonoBehaviour
{
    [field: SerializeField] public TMP_Text Subs { get; private set; }
    [field: SerializeField] public Transform TaskShowPlace { get; private set; }
    [field: SerializeField] public Transform TaskHidePLace { get; private set; }
    [field: SerializeField] public Transform Task { get; private set; }
    [field: SerializeField] public Image Target { get; private set; }
    [field: SerializeField] public Image HP { get; private set; }
    [field: SerializeField] public Image Armor{ get; private set; }
    [field: SerializeField] public Image Heart{ get; private set; }
    [field: SerializeField] public Image BloodScreen{ get; private set; }
}