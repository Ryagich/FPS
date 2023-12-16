using UnityEngine;
using EnemyAI;

// Decision to check if the target is dead.
[CreateAssetMenu(menuName = "Enemy AI/Decisions/Target Dead")]
public class TargetDeadDecision : Decision
{
    // The decide function, called on Update() (State controller - current state - transition - decision).
    public override bool Decide(StateController controller)
    {
        //var hm = controller.GetClosestTarget().root.GetComponent<HealthManager>();
        if (!controller.LastTarget)
            return false;
        var hm = controller.LastTarget.GetComponentInParent<HealthManager>();
        var stats = controller.LastTarget.GetComponentInParent<StatsController>();

        // Check dead condition on target health manager.
        return hm && hm.dead || stats && stats.Hp.Value <= .0f;
    }
}