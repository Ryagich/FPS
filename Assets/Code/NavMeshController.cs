using UnityEngine;
using Unity.AI.Navigation;

public class NavMeshController : MonoBehaviour
{
    [SerializeField] private NavMeshSurface _navMesh;

    public void Rebuild()
    {
        _navMesh.BuildNavMesh();
    }
}
