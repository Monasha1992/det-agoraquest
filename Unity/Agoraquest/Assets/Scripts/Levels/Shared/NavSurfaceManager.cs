using System.Collections;
using Unity.AI.Navigation;
using UnityEngine;

namespace Levels.Shared
{
    [RequireComponent(typeof(NavMeshSurface))]
    public class NavSurfaceManager : MonoBehaviour
    {
        private NavMeshSurface _mNavMeshSurface;

        private void Awake()
        {
            _mNavMeshSurface = GetComponent<NavMeshSurface>();
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            _mNavMeshSurface.BuildNavMesh();
            StartCoroutine(ExpandArea());
        }

        private IEnumerator ExpandArea()
        {
            while (_mNavMeshSurface.size.z < 12f)
            {
                yield return new WaitForSeconds(1f);

                // Increase the volume size's z-axis
                var newSize = _mNavMeshSurface.size;
                newSize.z += 1f;
                var newCenterZ = _mNavMeshSurface.center.z;
                newCenterZ += -0.5f;
                _mNavMeshSurface.center =
                    new Vector3(_mNavMeshSurface.center.x, _mNavMeshSurface.center.y, newCenterZ);
                _mNavMeshSurface.size = newSize;

                // Rebuild the NavMesh
                _mNavMeshSurface.UpdateNavMesh(_mNavMeshSurface.navMeshData);
            }
        }
    }
}