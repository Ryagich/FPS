using System;
using System.Collections.Generic;
using UnityEngine;
using EnemyAI;
using UnityEngine.Events;

[Serializable]
public struct EnemySpawnAndPatrolPoints
{
    public Transform Key;
    public List<Transform> Value;
}

[Serializable]
public class EnemyStage
{
    public UnityEvent EnemiesOver;
    public bool IsActive { get; private set; } = false;

    [field: SerializeField] public List<StateController> Enemies { get; private set; } = new();
    [field: SerializeField] public List<EnemySpawnAndPatrolPoints> Points{ get; private set; } = new();

    public void Activate(EnemyController enemyC)
    {
        IsActive = true;
        foreach (var element in Points)
        {
            var place = element.Key;
            var enemy = enemyC.InstantiateEnemy(place);
            if (element.Value.Count > 0)
            {
                enemy.SetPatrolPoints(element.Value);
            }
            enemy.GetComponent<EnemyHealth>().Dead.AddListener(() => RemoveEnemy(enemy));
            Enemies.Add(enemy);
        }
    }
    
    public void SetTarget(Transform target)
    {
        if (Enemies.Count == 0)
        {
            Debug.LogWarning("Ошибка получается");
        }
        else
        {
            foreach (var enemy in Enemies)
            {
                enemy.SetTarget(target);
            }
        }
    }

    public void RemoveTarget()
    {
        foreach (var enemy in Enemies)
        {
            enemy.RemoveTarget();
        }
    }

    private void RemoveEnemy(StateController enemy)
    {
        Enemies.Remove(enemy);
        CheckAliveEnemies();
    }

    private void CheckAliveEnemies()
    {
        foreach (var enemy in Enemies)
        {
            if (!enemy)
                continue;
            var eh = enemy.GetComponent<EnemyHealth>();
            if (eh && !eh.dead)
            {
                return;
            }
        }

        EndStage();
    }

    private void EndStage()
    {
        IsActive = false;
        EnemiesOver?.Invoke();
    }
}