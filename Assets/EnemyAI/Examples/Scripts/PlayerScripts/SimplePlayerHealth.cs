using System.Collections;
using EnemyAI;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

// This class is created for the example scene. There is no support for this script.
public class SimplePlayerHealth : HealthManager
{
    public static SimplePlayerHealth Instance;
    private StatsController statsC;

    private void Awake()
    {
        statsC = GetComponentInParent<StatsController>();

        statsC.Died += Dead;
        statsC.Respawned+= A;
    }

    public override void TakeDamage(Vector3 location, Vector3 direction, float damage,bool isPlayer, Collider bodyPart,
        GameObject origin)
    {
        statsC.TakeDamage(damage);
    }

    private void Dead()
    {
        dead = true;
    }

    private void A()
    {
        dead = false;
    }
}