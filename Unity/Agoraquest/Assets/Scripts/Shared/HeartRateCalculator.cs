using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Shared;

namespace Shared
{
    public class HeartRateCalculator : MonoBehaviour
    {
        // ���������������
        [SerializeField] private WristMonitorManager wristMonitor;

        // ��¼����
        private List<int> heartRateSamples = new List<int>();
        private float averageHeartRate;
        private Coroutine recordingCoroutine;
        private bool isRecording = false;

        // ������������
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
                Debug.Log("��ʼ��¼��������...");
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
                Debug.Log($"��¼��ɣ�ƽ������: {averageHeartRate:F1} BPM");
            }
        }

        private IEnumerator RecordingRoutine()
        {
            float timer = 0f;
            while (timer < recordingDuration)
            {
                timer += Time.deltaTime;

                // ʵʱ��ȡ��������ֵ
                int currentHR = wristMonitor.GetCurrentHeartRate();
                if (currentHR > 0) // ������Ч����֤
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
                    Debug.Log($"��ǰƽ������: {this.GetAverageHeartRate()} BPM");
                    Debug.Log($"��������: {this.GetRawData().Count}");
                }
               
            }
        }


        // �������ű���������
        public float GetAverageHeartRate() => averageHeartRate;
        public List<int> GetRawData() => new List<int>(heartRateSamples);
    }
}
