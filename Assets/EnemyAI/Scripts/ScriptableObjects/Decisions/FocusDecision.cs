using System.Linq;
using UnityEngine;
using EnemyAI;

// The decision to focus on the target.
[CreateAssetMenu(menuName = "Enemy AI/Decisions/Focus")]
public class FocusDecision : Decision
{
    [Tooltip("Which sense radius will be used?")]
    public Sense sense;

    [Tooltip("Invalidate current cover when target is spotted?")]
    public bool invalidateCoverSpot;

    private float radius; // The sense radius that will be used.

    // NPC Sense types.
    public enum Sense
    {
        NEAR,
        PERCEPTION,
        VIEW
    }

    // The decision on enable function, triggered once after a FSM state transition.
    public override void OnEnableDecision(StateController controller)
    {
        // Define sense radius.
        switch (sense)
        {
            case Sense.NEAR:
                radius = controller.nearRadius;
                break;
            case Sense.PERCEPTION:
                radius = controller.perceptionRadius;
                break;
            case Sense.VIEW:
                radius = controller.viewRadius;
                break;
        }
    }

    // The decide function, called on Update() (State controller - current state - transition - decision).
    public override bool Decide(StateController controller)
    {
        var targets = GetTargetsInRadius(controller, radius);
        var bestTarget = GetBestTarget(controller, targets);

        if (!bestTarget || !controller.variables.feelAlert)
            return false;

        if (invalidateCoverSpot)
            controller.CoverSpot = Vector3.positiveInfinity;
        controller.targetInSight = true;
        controller.LastTarget = bestTarget;
        controller.personalTarget = bestTarget.position;

        return true;
    }

    // The delegate for results of overlapping targets in focus decision.
    private static Transform GetBestTarget(StateController controller, Transform[] targets)
    {
        // Is there any target, with a clear sight to it?
        var filtered = targets;
            //.Where(target => controller.BlockedSight(target))
           // .ToArray();

        var min = float.MaxValue;
        Transform closest = null;
        foreach (var target in filtered)
        {
            var dis = controller.DistanceTo(target);
            if (dis < min)
            {
                min = dis;
                closest = target;
            }
        }
        //Debug.Log($"targets count {targets.Length}");
        //Debug.Log($"filtered count {filtered.Length}");
        //if (closest)
        //   Debug.Log(closest.name);
       // else
        //    Debug.Log("I havent closest");
        return closest;
    }
}