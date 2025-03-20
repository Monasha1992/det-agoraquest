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

            if (isLearning)
            {
                _npcSpawner.StartSpawning();
            }
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
            if (_wristMonitorManager.heartRateValue > AppStateData.MaxNormalHeartRateThreshold ||
                _wristMonitorManager.heartRateValue < AppStateData.MinNormalHeartRateThreshold)
            {
                AppNavigation.ToCalmingScene(CalmingScene.Forest);
            }
        }
    }
}