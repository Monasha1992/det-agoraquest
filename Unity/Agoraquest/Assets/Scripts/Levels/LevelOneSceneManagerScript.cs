using System.Collections;
using Levels.Shared;
using Shared;
using UnityEngine;

namespace Levels
{
    public class LevelOneSceneManagerScript : MonoBehaviour
    {
        private WristMonitorManager _wristMonitorManager;
        private NpcSpawner _npcSpawner;
        private int _heartRate;
        public GameObject learningWatch;
        public GameObject challengeWatch;
        public bool isGameInProgress;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            _wristMonitorManager = FindAnyObjectByType<WristMonitorManager>();
            _npcSpawner = FindAnyObjectByType<NpcSpawner>();
            var isLearning = AppStateData.GameMode == GameMode.Practice;
            learningWatch.SetActive(isLearning);
            challengeWatch.SetActive(!isLearning);
        }

        public void StartGame()
        {
            isGameInProgress = true;
            _npcSpawner.StartSpawning();
        }

        public void EndGame()
        {
            isGameInProgress = false;
            _npcSpawner.StopSpawning();
        }

        //Change scene when press panic button
        public void GoToCalming()
        {
            if (_heartRate > _wristMonitorManager.normalThreshold) AppNavigation.ToCalmingScene(CalmingScene.Forest);
        }

        public void UpdateHeartRate(int heartRate)
        {
            _heartRate = heartRate;
        }
    }
}