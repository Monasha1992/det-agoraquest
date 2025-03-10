using System;
using System.Collections;
using System.Collections.Generic;
using Shared;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class GameSelectorSceneManager : MonoBehaviour
    {
        public Button practiceButton;
        public Button challengeButton;
        public GameObject welcomeContent;
        public GameObject sceneSelectionContent;

        private void Start()
        {
            SetToPractice();
        }

        private void Update()
        {
            if (AppStateData.JourneyStarted && !sceneSelectionContent.activeSelf)
            {
                welcomeContent.SetActive(false);
                sceneSelectionContent.SetActive(true);
            }
        }
        
        public void StartJourney()
        {
            AppStateData.JourneyStarted = true;
        }

        public void StartGame()
        {
            // TODO Load the game scene
            Debug.Log("Starting game with mode: " + AppStateData.GameMode);
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