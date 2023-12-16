using System.Collections;
using System.Collections.Generic;
using EnemyAI;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    public static EnemyController Instance;
    [SerializeField] private List<StateController> _enemies;
    [SerializeField] private List<Transform> _places;
    [SerializeField] private StateController _enemy;
    [SerializeField, Min(.0f)] private float _respawnTime = 3f;
    [SerializeField, Min(0)] private int _enemyCount;
    [SerializeField] private bool _isDeathmatch = true;

    [SerializeField] private GameObject character;
    private bool characterIsAlive = true;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (_isDeathmatch && _enemies.Count > 0)
            foreach (var enemy in _enemies)
            foreach (var otherEnemy in _enemies)
                if (enemy != otherEnemy)
                    enemy.AddTarget(otherEnemy.transform);

        while (_enemies.Count != _enemyCount)
        {
            SpawnEnemy();
        }
    }

    public void SetCharacter()
    {
        characterIsAlive = true;
        foreach (var e in _enemies)
            e.AddTarget(character.transform);
    }

    public void SetCharacter(GameObject go)
    {
        character = go;
        SetCharacter();
    }

    public void RemoveCharacter()
    {
        characterIsAlive = false;
        foreach (var e in _enemies)
        {
            e.RemoveTarget(character.transform);
        }
    }

    public void SpawnEnemy(Transform place)
    {
        var enemy = Instantiate(_enemy, place.position, place.rotation);
        enemy.transform.SetParent(transform);
        if (character && characterIsAlive)
            enemy.AddTarget(character.transform);

        var enemyHealth = enemy.GetComponent<EnemyHealth>();
        var enemyTrans = enemy.transform;

        enemyHealth.Dead += () => StartCoroutine(Respawn());
        enemyHealth.Dead += () => _enemies.Remove(enemy);
        
        if (_isDeathmatch)
            foreach (var otherEnemy in _enemies)
            {
                otherEnemy.AddTarget(enemyTrans);
                enemyHealth.Dead += () => otherEnemy.RemoveTarget(enemyTrans);

                var otherTrans = otherEnemy.transform;
                enemy.AddTarget(otherTrans);
                otherEnemy.GetComponent<EnemyHealth>().Dead += () => enemy.RemoveTarget(otherTrans);
            }

        _enemies.Add(enemy);

        //Test
        enemy.patrolWayPoints = new List<Transform> { GetRandomPlace(), GetRandomPlace() };
    }

    public void SpawnEnemy()
    {
        var place = GetRandomPlace();
        SpawnEnemy(place);
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(_respawnTime);
        SpawnEnemy();
    }

    private Transform GetRandomPlace()
    {
        return _places[Random.Range(0, _places.Count)];
    }
}