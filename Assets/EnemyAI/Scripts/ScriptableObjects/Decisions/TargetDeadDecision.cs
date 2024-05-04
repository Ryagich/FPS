using UnityEngine;
using EnemyAI;

// Decision to check if the target is dead.
[CreateAssetMenu(menuName = "Enemy AI/Decisions/Target Dead")]
public class TargetDeadDecision : Decision
{
	// The decide function, called on Update() (State controller - current state - transition - decision).
	public override bool Decide(StateController controller)
	{
		try
		{
			// Check dead condition on target health manager.
			if (!controller.aimTarget)
				return false;
			var hm = controller.aimTarget.GetComponent<HealthManager>();
			var hmP =controller.aimTarget.GetComponentInParent<HealthManager>();
			return controller.aimTarget && ((hm && hm.dead) || (hmP && hmP.dead));
		}
		catch (UnassignedReferenceException)
		{
			// Ensure the target has a health manager set.
			Debug.LogError("Assign a health manager to" + controller.name);
		}
		return false;
	}
}
