using UnityEngine;

namespace Shared
{
    public class AppStateData : MonoBehaviour
    {
        public static bool JourneyStarted { get; set; }
        public static bool CalmSceneSelected { get; set; }
        public static bool HasHeartRateCalibrated { get; set; }
        public static GameMode GameMode { get; set; }
    }

    public enum GameMode
    {
        Challenge,
        Practice
    }
}