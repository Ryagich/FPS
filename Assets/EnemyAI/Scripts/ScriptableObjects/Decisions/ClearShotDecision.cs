using UnityEngine;
using EnemyAI;
using Unity.VisualScripting;

// The decision to check if sight to target is clear.
[CreateAssetMenu(menuName = "Enemy AI/Decisions/Clear Shot")]
public class ClearShotDecision : Decision
{
	[Header("Extra Decisions")]
	[Tooltip("The NPC near sense decision.")]
	public FocusDecision targetNear;

	public override bool Decide(StateController controller)
	{
		return targetNear.Decide(controller) || HaveClearShot(controller);
	}
	private bool HaveClearShot(StateController controller)
	{
		var shotOrigin = controller.transform.position + Vector3.up * (controller.generalStats.aboveCoverHeight + controller.nav.radius);
		var shotDirection = controller.personalTarget - shotOrigin;

		// Cast sphere in target direction to check for obstacles in near radius.
		var obscuredShot = Physics.SphereCast(shotOrigin, controller.nav.radius, shotDirection, out var hit,
			controller.nearRadius, controller.generalStats.coverMask | controller.generalStats.obstacleMask);
		if (!obscuredShot)
		{
			obscuredShot = Physics.Raycast(shotOrigin, shotDirection, out hit, shotDirection.magnitude,
				controller.generalStats.coverMask | controller.generalStats.obstacleMask);
			if (!controller.LastTarget)
				return false;
			if(obscuredShot)
				obscuredShot = !(hit.transform.root == controller.LastTarget.root);
		}
		return !obscuredShot;
	}
}
