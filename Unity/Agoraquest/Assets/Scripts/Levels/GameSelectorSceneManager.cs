using Shared;
using UnityEngine;
using UnityEngine.UI;

namespace Levels
{
    public class GameSelectorSceneManager : MonoBehaviour
    {
        public Button practiceButton;
        public Button challengeButton;
        public GameObject welcomeContent;
        public GameObject calmingSceneSelectionContent;
        public GameObject sceneSelectionContent;

        private int _calmingSceneId = 1;
        private int _stageId = 1;

        private void Start()
        {
            SetToPractice();
        }

        private void Update()
        {
            if (AppStateData.JourneyStarted && welcomeContent.activeSelf)
            {
                welcomeContent.SetActive(false);
                calmingSceneSelectionContent.SetActive(true);
                sceneSelectionContent.SetActive(false);
            }
            else if (AppStateData.CalmSceneSelected && calmingSceneSelectionContent.activeSelf)
            {
                welcomeContent.SetActive(false);
                calmingSceneSelectionContent.SetActive(false);
                sceneSelectionContent.SetActive(true);
            }
        }

        public void StartJourney()
        {
            AppStateData.JourneyStarted = true;
        }

        public void CalmingSceneSelected()
        {
            AppStateData.CalmSceneSelected = true;
        }

        public void StartGame()
        {
            AppNavigation.ToStage(_stageId);
        }

        public void SetToPractice()
        {
            AppStateData.GameMode = GameMode.Practice;

            practiceButton.colors = GetSelectedColors();
            challengeButton.colors = GetUnselectedColors();
        }

        public void SetToChallenge()
        {
            AppStateData.GameMode = GameMode.Challenge;

            challengeButton.colors = GetSelectedColors();
            practiceButton.colors = GetUnselectedColors();
        }

        public void SetCalmingSceneId(int calmingSceneId)
        {
            _calmingSceneId = calmingSceneId;
        }

        public void SetStageId(int stageId)
        {
            _stageId = stageId;
        }


        private ColorBlock GetSelectedColors()
        {
            // Set the color of the practiceButton
            var colors = practiceButton.colors;
            ColorUtility.TryParseHtmlString("#00EC2D", out var newColor);
            colors.normalColor = newColor;
            colors.highlightedColor = newColor;
            colors.pressedColor = newColor;
            colors.selectedColor = newColor;
            return colors;
        }

        private ColorBlock GetUnselectedColors()
        {
            // Set the color of the practiceButton
            var colors = practiceButton.colors;
            ColorUtility.TryParseHtmlString("#5C665D", out var newColor);
            colors.normalColor = newColor;
            colors.highlightedColor = newColor;
            colors.pressedColor = newColor;
            colors.selectedColor = newColor;
            return colors;
        }
    }
}