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
            //var hm = controller.GetClosestTarget().root.GetComponent<HealthManager>();
            var hm = controller.LastTarget.GetComponentInParent<HealthManager>();
            // Check dead condition on target health manager.
            return hm && hm.dead;
        }
        catch (UnassignedReferenceException)
        {
            // Ensure the target has a health manager set.
            Debug.LogError("Assign a health manager to" + controller.name);
        }

        return false;
    }
}