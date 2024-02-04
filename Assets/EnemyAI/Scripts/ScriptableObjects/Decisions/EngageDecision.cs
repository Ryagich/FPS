using UnityEngine;
using EnemyAI;

// The decision to keep engaging the current target position.
[CreateAssetMenu(menuName = "Enemy AI/Decisions/Engage")]
public class EngageDecision : Decision
{
    [Header("Extra Decisions")] [Tooltip("The NPC sight decision.")]
    public LookDecision isViewing;

    [Tooltip("The NPC near sense decision.")]
    public FocusDecision targetNear;

    // The decide function, called on Update() (State controller - current state - transition - decision).
    public override bool Decide(StateController controller)
    {
        var isV = isViewing.Decide(controller);
        var isN = targetNear.Decide(controller);
        if (!(isV || isN))
            Debug.Log($"isViewing {isV} targetNear {isN}");
        return isV || isN;
    }
}