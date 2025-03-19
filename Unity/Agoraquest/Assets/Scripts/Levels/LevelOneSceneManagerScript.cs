using Shared;
using UnityEngine;

namespace Levels
{
    public class LevelOneSceneManagerScript : MonoBehaviour
    {
        public GameObject learningWatch;
        public GameObject challengeWatch;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            var isLearning = AppStateData.GameMode == GameMode.Practice;
            learningWatch.SetActive(isLearning);
            challengeWatch.SetActive(!isLearning);
        }
    }
}