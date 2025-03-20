using UnityEngine;

namespace Shared
{
    public class AppStateData : MonoBehaviour
    {
        public const string MqttBroker = "broker.emqx.io";
        public const string MqttTopicHeartRateSensor = "sensor/heart_rate";

        public static bool JourneyStarted { get; set; }
        public static CalmingScene? SelectedCalmingScene { get; set; }
        public static bool HasHeartRateCalibrated { get; set; } = false;
        public static GameMode GameMode { get; set; }
    }

    public enum GameMode
    {
        Challenge,
        Practice,
    }

    public enum CalmingScene
    {
        Forest,
    }
}