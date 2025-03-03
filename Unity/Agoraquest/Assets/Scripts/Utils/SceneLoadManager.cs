using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

namespace Utils
{
    public class SceneLoadManager : MonoBehaviour
    {
        public GameObject progressUI;
        public Slider loadingBar;
        public TextMeshProUGUI progressValueText;

        public Image fadeImage;
        private const float FadeDuration = 1.0f;

        public void LoadScene(string sceneName)
        {
            StartCoroutine(LoadSceneAsync(sceneName));
        }

        /**
         * Load the scene asynchronously
         *
         * showing the progress of loading of the new scene with a progress bar
         */
        private IEnumerator LoadSceneAsync(string sceneName)
        {
            fadeImage.gameObject.SetActive(false); // Keep scene visible while loading

            var operation = SceneManager.LoadSceneAsync(sceneName);
            if (operation == null) yield break;

            operation.allowSceneActivation = false; // Prevent immediate activation


            // yield return new WaitForSeconds(1); // Optional delay
            fadeImage.gameObject.SetActive(true);
            StartCoroutine(FadeInBeforeActivation(operation)); // Start a 5-second fade-in
            // break;

            while (!operation.isDone)
            {
                progressUI.SetActive(true);
                var progress = Mathf.Clamp01(operation.progress / 0.9f);
                loadingBar.value = progress;
                progressValueText.text = progress * 100 + "%";

                // if (operation.progress >= 0.9f)
                // {
                //     // yield return new WaitForSeconds(1); // Optional delay
                //     fadeImage.gameObject.SetActive(true);
                //     StartCoroutine(FadeInBeforeActivation(operation)); // Start a 5-second fade-in
                //     break;
                // }

                yield return null;
            }
        }

        /**
         * Fade in to a black screen before activating the new scene
         *
         * will run for the duration of FadeDuration seconds
         * by increasing the alpha value of the image in every frame
         */
        private IEnumerator FadeInBeforeActivation(AsyncOperation operation)
        {
            float t = 0;
            while (t < FadeDuration)
            {
                t += Time.deltaTime;
                var color = fadeImage.color;
                color.a = t / FadeDuration;
                fadeImage.color = color;
                yield return null;
            }

            // Activate the new scene after fade-in completed
            operation.allowSceneActivation = true;
        }
    }
}