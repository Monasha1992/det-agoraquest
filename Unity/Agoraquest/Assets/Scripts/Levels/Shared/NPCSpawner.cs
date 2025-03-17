using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Levels.Shared
{
    public class NpcSpawner : MonoBehaviour
    {
        // public GameObject[] npcPrefabs;
        // public int maxNpcCount = 10; // Maximum NPCs in the scene at once
        // public float spawnInterval = 5f;
        // private int _currentNpcCount;
        // public Transform spawnPoint;
        //
        // private void Start()
        // {
        //     StartCoroutine(SpawnNpCs());
        // }
        //
        // private IEnumerator SpawnNpCs()
        // {
        //     while (_currentNpcCount < maxNpcCount)
        //     {
        //         yield return new WaitForSeconds(spawnInterval);
        //         var spawnPosition = new Vector3(
        //             Random.Range(-5f, 5f),
        //             0,
        //             Random.Range(-5f, 5f) // Adjust spawn area as needed
        //         );
        //
        //         // Randomly select a prefab from the array
        //         var npcPrefab = npcPrefabs[Random.Range(0, npcPrefabs.Length)];
        //         Instantiate(npcPrefab, spawnPosition, Quaternion.identity);
        //         _currentNpcCount++;
        //     }
        // }

        public GameObject[] npcPrefabs;
        public Transform spawnPoint;
        public float spawnInterval = 5f;
        public int npcLimit = 30;
        public List<NpcController> activeNpcList = new();

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            StartCoroutine(SpawnNpc());
        }

        private IEnumerator SpawnNpc()
        {
            while (activeNpcList.Count < npcLimit)
            {
                yield return new WaitForSeconds(spawnInterval);

                var npcPrefab = npcPrefabs[Random.Range(0, npcPrefabs.Length)];
                var newNpc = Instantiate(npcPrefab, spawnPoint.position, Quaternion.identity);
                var npcController = newNpc.GetComponent<NpcController>();
                activeNpcList.Add(npcController);
            }
        }
    }
}