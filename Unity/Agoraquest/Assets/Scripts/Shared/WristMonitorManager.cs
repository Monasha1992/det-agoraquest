using System;
using System.Collections;
using Levels;
using M2MqttUnity;
using TMPro;
using UnityEngine;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace Shared
{
    public class WristMonitorManager : M2MqttUnityClient
    {
        private LevelOneSceneManagerScript _levelOneSceneManagerScript;
        [Header("Heart Rate Value Labels")] public TextMeshProUGUI practiceHeartRateValueText;
        public TextMeshProUGUI challengeHeartRateValueText;

        [Header("Practice Mode")] public GameObject panicButtonIndicator;
        public GameObject panicTooltip;

        private bool _isInPanic;
        private float _elapsedTime;
        private float _heartRateValue;

        [Header("Heart Rate Thresholds")] public int lowThreshold = 65;
        public int normalThreshold = 72;
        public int highThreshold = 100;
        public int veryHighThreshold = 135;

        private const string DisabledColor = "#9A9A9A";
        private const string NormalColor = "#00C027";
        private const string HighColor = "#B45200";
        private const string VeryHighColor = "#B40600";
        private const string ExtremeHighColor = "#C00600";
        private const string LowColor = "#B40600";
        private const string ExtremeLowColor = "#C00600";


        [Header("Challenge Mode")] public TextMeshProUGUI spentTimeText;
        public TextMeshProUGUI challengeMaxHeartRateText;
        public TextMeshProUGUI challengeLevelText;
        public GameObject challengeDialog;
        public GameObject challengeColorFlickerDialog;
        public GameObject challengeGameStatusDialog;
        public TextMeshProUGUI gameStatusText;
        private bool _isChallengeRunning;
        private int _challengeLevel = 1;
        private int _challengeMaxHeartRate = 100;

        private DateTime? _lastChallengeFailedTime;

        protected override void Awake()
        {
            _levelOneSceneManagerScript = FindFirstObjectByType<LevelOneSceneManagerScript>();
            base.Awake();
        }

        private void OnValidate()
        {
            brokerAddress = AppStateData.MqttBroker;
            autoConnect = true;
        }

        protected override void Start()
        {
            if (AppStateData.GameMode == GameMode.Practice)
            {
                challengeDialog.SetActive(false);
                StartCoroutine(AnimateActionButton());
            }

            if (AppStateData.GameMode == GameMode.Challenge)
            {
                challengeDialog.SetActive(true);
            }

            const string heartRateValueText = "...";
            var heartRateValueTextColor = Color.gray;
            practiceHeartRateValueText.text = heartRateValueText;
            challengeHeartRateValueText.text = heartRateValueText;
            practiceHeartRateValueText.color = heartRateValueTextColor;
            challengeHeartRateValueText.color = heartRateValueTextColor;
            AddUiMessage("Ready.");

            base.Start();
        }

        public void StartChallenge()
        {
            challengeDialog.SetActive(false);
            _levelOneSceneManagerScript.StartGame();
            _isChallengeRunning = true;
            StartCoroutine(ChallengerWatch());
            StartCoroutine(UpdateSpentTime());
        }

        private void LateUpdate()
        {
            // check for fail
            if (_isChallengeRunning)
            {
                if (_challengeMaxHeartRate < _heartRateValue)
                {
                    if (_lastChallengeFailedTime == null
                       )
                    {
                        _lastChallengeFailedTime = DateTime.Now;
                        challengeColorFlickerDialog.SetActive(true);
                    }
                    // end the game if the player fails the challenge for more than 3 consecutive seconds
                    else if ((DateTime.Now - _lastChallengeFailedTime.Value).TotalSeconds > 3)
                    {
                        _isChallengeRunning = false;
                        _levelOneSceneManagerScript.EndGame();
                        challengeColorFlickerDialog.SetActive(false);
                        challengeGameStatusDialog.SetActive(true);
                        gameStatusText.text = "You Lost!";
                        gameStatusText.color = Color.red;
                    }
                }

                // if the player fails the challenge for more than 1 second, clear the last failed time
                else if (_lastChallengeFailedTime == null ||
                         (DateTime.Now - _lastChallengeFailedTime.Value).TotalSeconds > 1)
                {
                    _lastChallengeFailedTime = null;
                    challengeColorFlickerDialog.SetActive(false);
                }
            }
        }

        private IEnumerator ChallengerWatch()
        {
            while (_isChallengeRunning)
            {
                // level 10: 5 minutes 30 seconds to win
                if (_elapsedTime >= 330)
                {
                    // Win
                    _isChallengeRunning = false;
                    _levelOneSceneManagerScript.EndGame();

                    challengeGameStatusDialog.SetActive(true);
                    gameStatusText.text = "You Won!";
                    gameStatusText.color = Color.green;
                }
                else if (_elapsedTime > 300)
                {
                    _challengeLevel = 10;
                    _challengeMaxHeartRate = 80;
                }
                else if (_elapsedTime > 240)
                {
                    _challengeLevel = 9;
                    _challengeMaxHeartRate = 85;
                }
                else if (_elapsedTime > 210)
                {
                    _challengeLevel = 8;
                    _challengeMaxHeartRate = 85;
                }
                else if (_elapsedTime > 180)
                {
                    _challengeLevel = 7;
                    _challengeMaxHeartRate = 88;
                }
                else if (_elapsedTime > 150)
                {
                    _challengeLevel = 6;
                    _challengeMaxHeartRate = 90;
                }
                else if (_elapsedTime > 120)
                {
                    _challengeLevel = 5;
                    _challengeMaxHeartRate = 93;
                }
                else if (_elapsedTime > 90)
                {
                    _challengeLevel = 4;
                    _challengeMaxHeartRate = 95;
                }
                else if (_elapsedTime > 60)
                {
                    _challengeLevel = 3;
                    _challengeMaxHeartRate = 98;
                }
                else if (_elapsedTime > 30)
                {
                    _challengeLevel = 2;
                    _challengeMaxHeartRate = 95;
                }

                challengeMaxHeartRateText.text = _challengeMaxHeartRate.ToString();
                challengeLevelText.text = $"Level {_challengeLevel}";

                yield return new WaitForSeconds(1);
            }
        }

        public void ActivatePanicButton(bool isInPanic)
        {
            _isInPanic = isInPanic;
        }

        private IEnumerator AnimateActionButton()
        {
            var isBig = false;
            while (true)
            {
                panicTooltip.SetActive(_isInPanic);

                if (_isInPanic)
                {
                    // when in panic mode
                    // scale
                    panicButtonIndicator.transform.localScale = new Vector3(
                        isBig ? 9.5f : 10f,
                        isBig ? 0.25f : 0.5f,
                        isBig ? 9.5f : 10f
                    );
                    isBig = !isBig;
                }
                else
                {
                    // scale
                    panicButtonIndicator.transform.localScale = new Vector3(
                        panicButtonIndicator.transform.localScale.x,
                        0.05f,
                        panicButtonIndicator.transform.localScale.z
                    );
                }

                yield return new WaitForSeconds(0.5f);
            }
        }

        private IEnumerator UpdateSpentTime()
        {
            while (true)
            {
                _elapsedTime += 1;
                var timeSpan = TimeSpan.FromSeconds(_elapsedTime);
                spentTimeText.text = $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";

                // TODO: Save the time to the device
                yield return new WaitForSeconds(1f);
            }
        }

        protected override void SubscribeTopics()
        {
            client.Subscribe(new[] { AppStateData.MqttTopicHeartRateSensor },
                new[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        }

        protected override void UnsubscribeTopics()
        {
            client.Unsubscribe(new[] { AppStateData.MqttTopicHeartRateSensor });
        }

        protected override void DecodeMessage(string topic, byte[] message)
        {
            var msg = System.Text.Encoding.UTF8.GetString(message);

            if (int.TryParse(msg, out var heartRate))
            {
                // if the heart rate is over the normal threshold, vibrate the wrist
                var enableVibrate = heartRate < AppStateData.MinNormalHeartRateThreshold ||
                                    heartRate > AppStateData.MaxNormalHeartRateThreshold;
                Vibrate(enableVibrate);
                // activate panic button
                ActivatePanicButton(enableVibrate);

                var heartRateValueText = heartRate.ToString();
                practiceHeartRateValueText.text = heartRateValueText;
                challengeHeartRateValueText.text = heartRateValueText;
                _heartRateValue = heartRate;

                Color heartRateValueTextColor;
                if (heartRate < 0)
                {
                    ColorUtility.TryParseHtmlString(ExtremeLowColor, out heartRateValueTextColor);
                }
                else if (heartRate < AppStateData.MinNormalHeartRateThreshold)
                {
                    ColorUtility.TryParseHtmlString(LowColor, out heartRateValueTextColor);
                }
                else if (heartRate >= AppStateData.MinNormalHeartRateThreshold ||
                         heartRate <= AppStateData.MaxNormalHeartRateThreshold)
                {
                    ColorUtility.TryParseHtmlString(NormalColor, out heartRateValueTextColor);
                }
                else if (heartRate > AppStateData.MaxNormalHeartRateThreshold)
                {
                    ColorUtility.TryParseHtmlString(HighColor, out heartRateValueTextColor);
                }
                else
                {
                    ColorUtility.TryParseHtmlString(ExtremeHighColor, out heartRateValueTextColor);
                }

                practiceHeartRateValueText.color = heartRateValueTextColor;
                challengeHeartRateValueText.color = heartRateValueTextColor;
            }
            else
            {
                _heartRateValue = 0;
                // deactivate panic button
                ActivatePanicButton(false);


                const string heartRateValueText = "!!!";
                var heartRateValueTextColor =
                    ColorUtility.TryParseHtmlString(DisabledColor, out var color)
                        ? color
                        : Color.gray;
                practiceHeartRateValueText.text = heartRateValueText;
                challengeHeartRateValueText.text = heartRateValueText;
                practiceHeartRateValueText.color = heartRateValueTextColor;
                challengeHeartRateValueText.color = heartRateValueTextColor;
            }
        }

        private void Vibrate(bool isActivate)
        {
            client.Publish(AppStateData.MqttTopicVibration,
                System.Text.Encoding.UTF8.GetBytes(isActivate ? "activate" : "deactivate"),
                MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        }

        private static void AddUiMessage(string msg, bool isError = false)
        {
            if (isError) Debug.LogError("MQTT: " + msg);
            else Debug.Log("MQTT: " + msg);
        }

        protected override void OnConnecting()
        {
            base.OnConnecting();
            AddUiMessage("Connecting to broker on " + brokerAddress + ":" + brokerPort);
        }

        protected override void OnConnected()
        {
            base.OnConnected();
            AddUiMessage("Connected to broker on " + brokerAddress);
        }

        protected override void OnConnectionFailed(string errorMessage)
        {
            AddUiMessage("CONNECTION FAILED! " + errorMessage, true);
        }

        protected override void OnDisconnected()
        {
            AddUiMessage("Disconnected.");
        }

        protected override void OnConnectionLost()
        {
            AddUiMessage("CONNECTION LOST!", true);
        }


        private void OnDestroy()
        {
            Disconnect();
        }
    }
}