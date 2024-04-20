using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Levels")]
public class Levels : ScriptableObject
{
    [field: SerializeField] public GameObject Stage_0 { get; private set; }
    [field: SerializeField] public List<GameObject> Stage_1 { get; private set; }
    [field: SerializeField] public List<GameObject> Stage_2 { get; private set; }
    [field: SerializeField] public List<GameObject> Stage_3 { get; private set; }
    [field: SerializeField] public List<GameObject> Stage_4 { get; private set; }
}
