using InfimaGames.LowPolyShooterPack;
using UnityEngine;
using UnityEngine.Events;

public class TriggerZone : MonoBehaviour
{
    [SerializeField] public UnityEvent<GameObject> _entered;
    
    private void OnTriggerEnter(Collider other)
    {
        var hero = other.GetComponent<Character>();
        if (!hero)
            return;
        
        _entered?.Invoke(other.gameObject);
    }
}
