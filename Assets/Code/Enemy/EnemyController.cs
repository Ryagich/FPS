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
        var statC = target.GetComponent<StatsController>();
        if (statC)
        {
            statC.Died += RemoveCharacter;
        }
        
        foreach (var stage in _stages)
        {
            if (stage.IsActive)
            {
                stage.SetTarget(Target);
            }
        }
    }

    public void AttackEnemy()
    {
        foreach (var stage in _stages)
        {
            if (stage.IsActive)
            {
                List<StateController> scs = new();
                var enemyTarget = stage.Enemies[Random.Range(0, stage.Enemies.Count - 1)];
                enemyTarget.GetComponent<EnemyHealth>().Dead.AddListener(() =>
                {
                    foreach (var stage in _stages)
                    {
                        if (stage.IsActive)
                        {
                            stage.SetTarget(Target);
                        }
                    }
                });
                foreach (var enemy in stage.Enemies)
                {
                    if (enemy != enemyTarget)
                    {
                        enemy.SetTarget(enemyTarget.GetComponent<TargetHolder>().Target);
                    }
                }
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
        {
            enemy.SetTarget(Target);
        }
        return enemy;
    }

    public void StartStage(int index)
    {
        if (_stages.Count <= index)
            Debug.LogError("Ну ты косяк пиздец");
        _stages[index].Activate(this);
    }
}