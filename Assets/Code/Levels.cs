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

    public List<GameObject> GetStage(int i)
    {
        return i switch
               {
                   1 => new List<GameObject>(Stage_1),
                   2 => new List<GameObject>(Stage_2),
                   3 => new List<GameObject>(Stage_3),
                   4 => new List<GameObject>(Stage_4),
                   _ => null
               };
    }
}
