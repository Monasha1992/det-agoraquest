using System.Collections;
using Shared;
using UnityEngine;

namespace CalmingScenes
{
    public class CalmingSceneManager : MonoBehaviour
    {
        public GameObject calibrationInfoDialog;
        public GameObject calibrationDialog;
        public GameObject calibrationDoneDialog;
        private WristHeartRateCalibrator _wristHeartRateCalibrator;
        private CalmingGuidance _calmingGuidance;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            _calmingGuidance = FindAnyObjectByType<CalmingGuidance>();
            _wristHeartRateCalibrator = FindAnyObjectByType<WristHeartRateCalibrator>();
            calibrationInfoDialog.SetActive(!AppStateData.HasHeartRateCalibrated);
            // _wristHeartRateCalibrator.StartRecording();
        }

        public void StartCalibration()
        {
            calibrationInfoDialog.SetActive(false);
            calibrationDialog.SetActive(true);
            _wristHeartRateCalibrator.StartRecording();
            _calmingGuidance.StartGuidance();
        }

        public void EndCalibration()
        {
            calibrationDialog.SetActive(false);
            calibrationDoneDialog.SetActive(true);
            AppStateData.HasHeartRateCalibrated = true;
            StartCoroutine(NavigateToNextScene());
        }

        private IEnumerator NavigateToNextScene()
        {
            yield return new WaitForSeconds(2f);
            AppNavigation.ToStage(1);
        }
    }
}