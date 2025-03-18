using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Shared;

namespace Shared
{
    public class HeartRateCalculator : MonoBehaviour
    {
        // 引用手腕监测管理器
        [SerializeField] private WristMonitorManager wristMonitor;

        // 记录参数
        private List<int> heartRateSamples = new List<int>();
        private float averageHeartRate;
        private Coroutine recordingCoroutine;
        private bool isRecording = false;

        // 场景触发配置
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
                Debug.Log("开始记录心率数据...");
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
                Debug.Log($"记录完成，平均心率: {averageHeartRate:F1} BPM");
            }
        }

        private IEnumerator RecordingRoutine()
        {
            float timer = 0f;
            while (timer < recordingDuration)
            {
                timer += Time.deltaTime;

                // 实时获取最新心率值
                int currentHR = wristMonitor.GetCurrentHeartRate();
                if (currentHR > 0) // 基本有效性验证
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
                    Debug.Log($"当前平均心率: {this.GetAverageHeartRate()} BPM");
                    Debug.Log($"样本数量: {this.GetRawData().Count}");
                }
               
            }
        }


        // 供其他脚本访问数据
        public float GetAverageHeartRate() => averageHeartRate;
        public List<int> GetRawData() => new List<int>(heartRateSamples);
    }
}
