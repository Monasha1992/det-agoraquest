using System.Collections;
using System.Collections.Generic;
using M2MqttUnity;
using Shared;
using UnityEngine;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace CalmingScenes
{
    public class WristHeartRateCalibrator : M2MqttUnityClient
    {
        // Call Wrist Monitoring Manager
        //[SerializeField] private WristMonitorManager wristMonitor;

        // Parameters
        private List<int> heartRateSamples = new List<int>();
        private float averageHeartRate;
        private Coroutine recordingCoroutine;
        private bool isRecording = false;

        // Scene Trigger Configuration
        [SerializeField] private string targetSceneName = "ForestCalmingScene";
        [SerializeField] private float recordingDuration = 60f;

        private int currentHeartRate;

        [Header("MQTT Publish Setting")] public string averageHeartRateTopic = "sensor/average_heart_rate";
        //public TextMeshProUGUI heartRateValueText;


        private static void AddUiMessage(string msg)
        {
            Debug.Log("===============> " + msg);
        }

        protected override void OnConnecting()
        {
            base.OnConnecting();
            AddUiMessage("Connecting to broker on " + brokerAddress + ":" + brokerPort.ToString() + "...\n");
        }

        protected override void OnConnected()
        {
            base.OnConnected();
            AddUiMessage("Connected to broker on " + brokerAddress + "\n");
        }

        protected override void SubscribeTopics()
        {
            client.Subscribe(new[] { "sensor/heart_rate" }, new[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        }

        protected override void UnsubscribeTopics()
        {
            client.Unsubscribe(new[] { "sensor/heart_rate" });
        }

        protected override void OnConnectionFailed(string errorMessage)
        {
            AddUiMessage("CONNECTION FAILED! " + errorMessage);
        }

        protected override void OnDisconnected()
        {
            AddUiMessage("Disconnected.");
        }

        protected override void OnConnectionLost()
        {
            AddUiMessage("CONNECTION LOST!");
        }

        protected override void Start()
        {
            autoConnect = true;
            base.Start();
        }

        public int GetCurrentHeartRate()
        {
            return currentHeartRate;
        }

        public void SendAverageHeartRate(float average)
        {
            if (client != null && client.IsConnected)
            {
                try
                {
                    string payload = average.ToString("F1");
                    byte[] message = System.Text.Encoding.UTF8.GetBytes(payload);
                    client.Publish(averageHeartRateTopic, message, MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
                    AddUiMessage($"Average heart rate sent: {payload} BPM");
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"Send Failure: {e.Message}");
                }
            }
            else
            {
                Debug.LogWarning("MQTT client is not connected and cannot send data");
            }
        }

        protected override void DecodeMessage(string topic, byte[] message)
        {
            var msg = System.Text.Encoding.UTF8.GetString(message);

            if (int.TryParse(msg, out var heartRate))
            {
                currentHeartRate = heartRate;
            }
            else
            {
                currentHeartRate = 0; // meaningless data
            }
        }


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
            if (!isRecording)
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
                if (averageHeartRate != 0f)
                {
                    SendAverageHeartRate(averageHeartRate);
                }

                Debug.Log($"Recording complete, average heart rate: {averageHeartRate:F1} BPM");

                Disconnect();
                AppNavigation.ToStage(1);
            }
        }

        private IEnumerator RecordingRoutine()
        {
            float timer = 0f;
            while (timer < recordingDuration)
            {
                timer += Time.deltaTime;

                // get current heart rate
                int currentHR = GetCurrentHeartRate();
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

        private void OnDestroy()
        {
            Disconnect();
        }
    }
}