using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Shared;

namespace Shared
{
    public class HeartRateCalculator : MonoBehaviour
    {
        // Call Wrist Monitoring Manager
        [SerializeField] private WristMonitorManager wristMonitor;

        // Parameters
        private List<int> heartRateSamples = new List<int>();
        private float averageHeartRate;
        private Coroutine recordingCoroutine;
        private bool isRecording = false;

        // Scene Trigger Configuration
        [SerializeField] private string targetSceneName = "Stage1";
        [SerializeField] private float recordingDuration = 60f;

        void OnEnable()
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnDisable()
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene,
                                  UnityEngine.SceneManagement.LoadSceneMode mode)
        {
            if (scene.name == targetSceneName)
            {
                StartRecording();
            }
            else
            {
                StopRecording();
            }
        }

        public void StartRecording()
        {
            if (!isRecording && wristMonitor != null)
            {
                heartRateSamples.Clear();
                isRecording = true;
                recordingCoroutine = StartCoroutine(RecordingRoutine());
                Debug.Log("Start recording heart rate data...");
            }
        }

        public void StopRecording()
        {
            if (isRecording)
            {
                if (recordingCoroutine != null)
                {
                    StopCoroutine(recordingCoroutine);
                }
                isRecording = false;
                CalculateAverage();
                Debug.Log($"Recording complete, average heart rate: {averageHeartRate:F1} BPM");
            }
        }

        private IEnumerator RecordingRoutine()
        {
            float timer = 0f;
            while (timer < recordingDuration)
            {
                timer += Time.deltaTime;

                // get current heart rate
                int currentHR = wristMonitor.GetCurrentHeartRate();
                if (currentHR > 0) // validate
                {
                    heartRateSamples.Add(currentHR);
                }
                Debug.Log($"Current Heart Rate is: {currentHR}");
                yield return null;
            }
            StopRecording();
        }

        private void CalculateAverage()
        {
            if (heartRateSamples.Count == 0)
            {
                averageHeartRate = 0f;
                return;
            }

            int total = 0;
            foreach (var hr in heartRateSamples)
            {
                total += hr;
            }
            averageHeartRate = (float)total / heartRateSamples.Count;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartRecording();
                if(isRecording == false)
                {
                    Debug.Log($"Current average heart rate: {this.GetAverageHeartRate()} BPM");
                    Debug.Log($"sample size: {this.GetRawData().Count}");
                }
               
            }
        }


        // For other scripts to access the data (if needed)
        public float GetAverageHeartRate() => averageHeartRate;
        public List<int> GetRawData() => new List<int>(heartRateSamples);
    }
}
