using System.Linq;
using UnityEngine;
using EnemyAI;

// The decision to see the target. Sense of sight.
[CreateAssetMenu(menuName = "Enemy AI/Decisions/Look")]
public class LookDecision : Decision
{
	// The decide function, called on Update() (State controller - current state - transition - decision).
	public override bool Decide(StateController controller)
	{
		// Reset sight status on loop before checking.
		controller.targetInSight = false;
		// Check sight.
		var targetsToCheck = GetTargetsInRadius(controller, controller.viewRadius);
		var bestTarget = GetBestTarget(controller, targetsToCheck);
		if (!bestTarget)
			return false;
		// Set current target parameters.
		controller.targetInSight = true;
		controller.LastTarget = bestTarget;
		//controller.personalTarget = controller.GetClosestTarget().position;
		controller.personalTarget = bestTarget.position;
		return true;
	}

	// The delegate for results of overlapping targets in look decision.
	private static Transform GetBestTarget(StateController controller, Transform[] targetsInViewRadius)
	{
		var controllerTransform = controller.transform;
		var filtered = targetsInViewRadius
			.Where(target =>
		{
			// Check if target is in field of view.
			var dirToTarget = target.position - controllerTransform.position;
			//var dirToTarget = Vector3.Distance(target, controller.transform.position);
			var inFOVCondition = (Vector3.Angle(controllerTransform.forward, dirToTarget) < controller.viewAngle / 2);
			// Is target in FOV and NPC have a clear sight?
			
			return inFOVCondition && !controller.BlockedSight(target);
		})
			.ToArray();
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
		return closest;
	}
}
