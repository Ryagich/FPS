using System.Collections.Generic;
using EnemyAI;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public static EnemyController Instance { get; private set; }
    private List<StateController> enemyStatesC = new();

    private void Awake()
    {
        Instance = this;
    }

    public void SetCharacter(GameObject target)
    {
        var t = target.GetComponent<TargetHolder>().Target;
        foreach (var en in enemyStatesC)
        {
            en.SetTarget(t);
        }
    }

    public void RemoveCharacter()
    {
        foreach (var en in enemyStatesC)
        {
            en.RemoveTarget();
        }
    }
}