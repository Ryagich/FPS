using System.Collections.Generic;
using NUnit.Framework;
using System.Linq;
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
        protected static Transform[] GetTargetsInRadius(StateController controller, float radius)
        {
            return controller.Targets
                .Where(target => controller.DistanceTo(target) < radius)
                .ToArray();
        }
    }
}