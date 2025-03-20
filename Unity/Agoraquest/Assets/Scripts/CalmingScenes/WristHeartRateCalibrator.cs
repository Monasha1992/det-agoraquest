using System.Collections;
using System.Collections.Generic;
using M2MqttUnity;
using TMPro;
using UnityEngine;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace CalmingScenes
{
    public class WristHeartRateCalibrator : M2MqttUnityClient
    {
        public TextMeshProUGUI heartRateValueText;
        public TextMeshProUGUI averageHeartRateValueText;
        public TextMeshProUGUI timeRemainingText;

        private CalmingSceneManager _calmingSceneManager;
        // Call Wrist Monitoring Manager
        //[SerializeField] private WristMonitorManager wristMonitor;

        // Parameters
        private readonly List<int> _heartRateSamples = new();
        private float _averageHeartRate;
        private Coroutine _recordingCoroutine;
        private bool _isRecording;

        // Scene Trigger Configuration
        [SerializeField] private string targetSceneName = "ForestCalmingScene";
        [SerializeField] private float recordingDuration = 60f;

        private int _currentHeartRate;

        [Header("MQTT Publish Setting")] public string averageHeartRateTopic = "sensor/average_heart_rate";
        //public TextMeshProUGUI heartRateValueText;

        protected override void Awake()
        {
            base.Awake();

            _calmingSceneManager = FindAnyObjectByType<CalmingSceneManager>();
        }

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
            return _currentHeartRate;
        }

        public void SendAverageHeartRate(float average)
        {
            if (client != null && client.IsConnected)
            {
                try
                {
                    var payload = average.ToString("F0");
                    var message = System.Text.Encoding.UTF8.GetBytes(payload);
                    client.Publish(averageHeartRateTopic, message, MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
                    AddUiMessage($"Average heart rate sent: {payload} BPM");
                    averageHeartRateValueText.text = payload;
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
                _currentHeartRate = heartRate;
            }
            else
            {
                _currentHeartRate = 0; // meaningless data
            }

            heartRateValueText.text = _currentHeartRate.ToString();
        }


        // void OnEnable()
        // {
        //     UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
        // }
        //
        // void OnDisable()
        // {
        //     UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
        // }

        // private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene,
        //     UnityEngine.SceneManagement.LoadSceneMode mode)
        // {
        //     if (scene.name == targetSceneName)
        //     {
        //         StartRecording();
        //     }
        //     else
        //     {
        //         StopRecording();
        //     }
        // }

        public void StartRecording()
        {
            if (!_isRecording)
            {
                _heartRateSamples.Clear();
                _isRecording = true;
                _recordingCoroutine = StartCoroutine(RecordingRoutine());
                Debug.Log("Start recording heart rate data...");
            }
        }

        public void StopRecording()
        {
            if (_isRecording)
            {
                if (_recordingCoroutine != null)
                {
                    StopCoroutine(_recordingCoroutine);
                }

                _isRecording = false;
                CalculateAverage();
                if (_averageHeartRate != 0f)
                {
                    SendAverageHeartRate(_averageHeartRate);
                }

                Debug.Log($"Recording complete, average heart rate: {_averageHeartRate:F0} BPM");

                Disconnect();
                _calmingSceneManager.EndCalibration();
            }
        }

        private IEnumerator RecordingRoutine()
        {
            var timer = 0f;
            while (timer < recordingDuration)
            {
                Debug.Log("Recording..." + timer);
                // timer += Time.deltaTime;
                yield return new WaitForSeconds(1f);
                timer += 1;

                // get current heart rate
                var currentHr = GetCurrentHeartRate();
                if (currentHr > 0) // validate
                {
                    _heartRateSamples.Add(currentHr);
                }

                Debug.Log($"Current Heart Rate is: {currentHr}");

                CalculateAverage();
                timeRemainingText.text = (recordingDuration - timer).ToString("F0") + " seconds";
                averageHeartRateValueText.text = _averageHeartRate.ToString("F0");
                yield return null;
            }

            StopRecording();
        }

        private void CalculateAverage()
        {
            if (_heartRateSamples.Count == 0)
            {
                _averageHeartRate = 0f;
                return;
            }

            var total = 0;
            foreach (var hr in _heartRateSamples)
            {
                total += hr;
            }

            _averageHeartRate = (float)total / _heartRateSamples.Count;
        }

        //private void OnDestroy()
        //{
        //    Disconnect();
        //}
    }
}