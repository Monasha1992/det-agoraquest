using M2MqttUnity;
using TMPro;
using UnityEngine;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace Shared
{
    public class WristMonitorManager : M2MqttUnityClient
    {
        private const string DisabledColor = "#9A9A9A";
        private const string NormalColor = "#00C027";
        private const string HighColor = "#B45200";
        private const string VeryHighColor = "#B40600";
        private const string ExtremeHighColor = "#C00600";
        private const string LowColor = "#B40600";
        private const string ExtremeLowColor = "#C00600";
        private int currentHeartRate;

        public TextMeshProUGUI heartRateValueText;


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
            heartRateValueText.text = "...";
            heartRateValueText.color = Color.gray;
            AddUiMessage("Ready.");
            base.Start();
        }

        public int GetCurrentHeartRate()
        {
            return currentHeartRate;
        }

        protected override void DecodeMessage(string topic, byte[] message)
        {
            var msg = System.Text.Encoding.UTF8.GetString(message);

            if (int.TryParse(msg, out var heartRate))
            {
                currentHeartRate = heartRate;
                heartRateValueText.text = heartRate.ToString();
                ColorUtility.TryParseHtmlString(heartRate switch
                {
                    < 0 => ExtremeLowColor,
                    < 60 => LowColor,
                    < 100 => NormalColor,
                    < 120 => HighColor,
                    < 140 => VeryHighColor,
                    _ => ExtremeHighColor
                }, out var color);
                heartRateValueText.color = color;
            }
            else
            {
                currentHeartRate = 0; // meaningless data
                heartRateValueText.text = "!!!";
                heartRateValueText.color = ColorUtility.TryParseHtmlString( DisabledColor, out var color)
                    ? color
                    : Color.gray;
            }
        }

        private void OnDestroy()
        {
            Disconnect();
        }
    }
}