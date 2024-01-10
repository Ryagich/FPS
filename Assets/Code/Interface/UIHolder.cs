using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHolder : MonoBehaviour
{
    public static UIHolder Instance;

    private void Awake()
    {
        Instance = this;
    }

    [field: SerializeField] public TMP_Text Subs { get; private set; }
    [field: SerializeField] public Transform TaskShowPlace { get; private set; }
    [field: SerializeField] public Transform TaskHidePLace { get; private set; }
    [field: SerializeField] public Transform Task { get; private set; }
    [field: SerializeField] public Image Hide { get; private set; }
    [field: SerializeField] public Image Target { get; private set; }
    [field: SerializeField] public Image HP { get; private set; }
    [field: SerializeField] public Image Armor{ get; private set; }
    [field: SerializeField] public Image Heart{ get; private set; }
    [field: SerializeField] public Image BloodScreen{ get; private set; }
    [field: SerializeField] public Transform StartKillCallbackPoint { get; private set; }
    [field: SerializeField] public Transform CallbackParent { get; private set; }

}