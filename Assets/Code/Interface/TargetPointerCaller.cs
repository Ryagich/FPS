using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPointerCaller : MonoBehaviour
{
    public void SetTarget(Transform newTarget)
    {
        TargetPointer.Instance.SetTarget(newTarget);
    }
    
    public void RemoveTarget()
    {
        TargetPointer.Instance.RemoveTarget();
    }
}
