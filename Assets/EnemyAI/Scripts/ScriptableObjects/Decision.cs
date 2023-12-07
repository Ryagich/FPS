using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace EnemyAI
{
    // A template scriptable object for a FSM decision.
    // Any custom FSM decision must inherit from this class.
    public abstract class Decision : ScriptableObject
    {
        // The decide function, called on Update() (State controller - current state - transition - decision).
        public abstract bool Decide(StateController controller);

        // The decision on enable function, triggered once after a FSM state transition.
        public virtual void OnEnableDecision(StateController controller)
        {
        }

        // The common overlap function for senses decisions (look, hear, near, etc.)
        public static bool CheckTargetsInRadius(StateController controller, float radius, HandeTargets handleTargets)
        {
            if (controller.Targets.Count == 0)
                return false;
            var closestTargetInRadius = controller.GetClosestAliveTargetInRadius(radius);
            if (!closestTargetInRadius)
                return false;
            return handleTargets(controller, closestTargetInRadius, new[] { closestTargetInRadius.GetComponent<Collider>() });

            // Target is dead, ignore sense triggers.
            //var hm = controller.GetClosestTarget().root.GetComponent<HealthManager>();
            //if (hm && hm.dead)
            //   return false;
            // Target is alive.
            //else
            //Collider[] allTargetsInRadius =
            //    Physics.OverlapSphere(controller.transform.position, radius, controller.generalStats.targetMask);
            //var colliders = new List<Collider>();
            //foreach (var tc in allTargetsInRadius)
            //{
            //    if (!controller.MyColliders.Contains(tc) && controller.Targets.Contains(tc.transform))
            //    {
            //        colliders.Add(tc);
            //    }
            // }
            //if (colliders.Count == 0)
            //   return false;
            // var Min = float.MaxValue;
            // var t = colliders[0];
            // foreach (var collider in colliders)
            // {
            //     var d = Vector3.Distance(collider.transform.position, controller.transform.position);
            //     if (d < Min)
            //     {
            //         Min = d;
            //         t = collider;
            //     }
            // }
            // return handleTargets(controller, newTargets.Count > 0, newTargets.ToArray());
            // return handleTargets(controller, newTargets.Count > 0, new[] { controller.GetClosestTarget().GetComponent<TargetHolder>().Target.GetComponent<Collider>() });
            //return handleTargets(controller, colliders.Count > 0, new[] { controller.GetClosestTarget().GetComponent<Collider>() });
        }

        // The delegate for results of overlapping targets in senses decisions.
        public delegate bool HandeTargets(StateController controller, bool hasTargets, Collider[] targetsInRadius);
    }
}