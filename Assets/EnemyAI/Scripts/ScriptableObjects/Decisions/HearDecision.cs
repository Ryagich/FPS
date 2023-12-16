using UnityEngine;
using EnemyAI;
using System.Linq;
    
// The decision to hear an evidence. Sense of hearing.
[CreateAssetMenu(menuName = "Enemy AI/Decisions/Hear")]
public class HearDecision : Decision
{
    private Vector3 lastPos, currentPos; // Last and current evidence positions.

    // The decide function, called on Update() (State controller - current state - transition - decision).
    public override bool Decide(StateController controller)
    {
        // Handle external alert received.
        if (controller.variables.hearAlert)
        {
            controller.variables.hearAlert = false;
            return true;
        }
        
        var targetsToCheck = GetTargetsInRadius(controller, controller.perceptionRadius);
        var bestTarget = GetBestTarget(controller, targetsToCheck);

        if (!bestTarget)
            return false;
        controller.LastTarget = bestTarget;
        controller.personalTarget = bestTarget.position;
        // controller.targetInSight = true;
        // Check if something was heard by the NPC.
        return true;
    }

    // The delegate for results of overlapping targets in hear decision.
    private static Transform GetBestTarget(StateController controller, Transform[] targetsInHearRadius)
    {
        var min = float.MaxValue;
        Transform closest = null;
        foreach (var target in targetsInHearRadius)
        {
            var dis = controller.DistanceTo(target);
            if (dis < min)
            {
                min = dis;
                closest = target;
            }
        }
        return closest;
    }
}