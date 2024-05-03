using System.Collections.Generic;
using EnemyAI;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public static EnemyController Instance { get; private set; }
    [SerializeField] private StateController _enemyPref;
    [SerializeField] private List<EnemyStage> _stages = new();

    public Transform Target { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void SetCharacter(GameObject target)
    {
        Target = target.GetComponent<TargetHolder>().Target;
        foreach (var stage in _stages)
        {
            if (stage.IsActive)
            {
                stage.SetTarget(Target);
            }
        }
    }

    public void RemoveCharacter()
    {
        foreach (var stage in _stages)
        {
            if (stage.IsActive)
            {
                stage.RemoveTarget();
            }
        }
    }

    public StateController InstantiateEnemy(Transform place)
    {
        var enemy = Instantiate(_enemyPref, place.position, place.rotation);
        if (Target)
            enemy.SetTarget(Target);
        return enemy;
    }

    public void StartStage(int index)
    {
        if (_stages.Count <= index)
            Debug.LogError("Ну ты косяк пиздец");
        _stages[index].Activate(this);
    }
}