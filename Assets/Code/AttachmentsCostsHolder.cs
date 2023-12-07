using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachmentsCostsHolder : MonoBehaviour
{
    public static AttachmentsCostsHolder Instance;
    [field: SerializeField]
    public int[] CostsScopes { get; private set; } = { 500, 800, 1200, 1500, 1750, 1900, 2100, 2500, };
    [field: SerializeField]
    public int[] CostsMuzzle { get; private set; } = { 400, 800, 1100, 1400, };
    [field: SerializeField]
    public int[] CostsLaser { get; private set; } = { 200, 500, };
    [field: SerializeField]
    public int[] CostsGrip { get; private set; } = { 700, 900, 1200 };

    private void Awake()
    {
        Instance = this;
    }
}