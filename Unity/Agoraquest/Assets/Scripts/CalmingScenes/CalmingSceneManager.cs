using Shared;
using UnityEngine;

namespace CalmingScenes
{
    public class CalmingSceneManager : MonoBehaviour
    {
        public GameObject calibrationInfoDialog;
        public GameObject calibrationDialog;
        private WristHeartRateCalibrator _wristHeartRateCalibrator;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            _wristHeartRateCalibrator = FindAnyObjectByType<WristHeartRateCalibrator>();
            // calibrationInfoDialog.SetActive(!AppStateData.HasHeartRateCalibrated);
            _wristHeartRateCalibrator.StartRecording();
        }

        public void StartCalibration()
        {
            calibrationInfoDialog.SetActive(false);
            calibrationDialog.SetActive(true);
            _wristHeartRateCalibrator.StartRecording();
        }

        public void EndCalibration()
        {
        }
    }
}