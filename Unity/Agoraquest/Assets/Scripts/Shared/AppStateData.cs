using UnityEngine;

namespace Shared
{
    public class AppStateData : MonoBehaviour
    {
        public const string MqttBroker = "broker.emqx.io";
        public const string MqttTopicHeartRateSensor = "sensor/heart_rate";
        public const string MqttTopicVibration = "esp32/control";

        public static bool JourneyStarted { get; set; }
        public static CalmingScene? SelectedCalmingScene { get; set; }
        public static bool HasHeartRateCalibrated { get; set; } = false;
        public static int MinNormalHeartRateThreshold { get; set; } = 80;
        public static int MaxNormalHeartRateThreshold { get; set; } = 68;
        public static GameMode GameMode { get; set; }
    }

    public enum GameMode
    {
        Practice,
        Challenge,
    }

    public enum CalmingScene
    {
        Forest,
    }
}