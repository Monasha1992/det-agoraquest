using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

namespace Utils
{
    public class SceneLoadManager : MonoBehaviour
    {
        public Slider loadingBar;
        public TextMeshProUGUI progressText;

        public Image fadeImage;
        private const float FadeDuration = 2.0f;

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
            var operation = SceneManager.LoadSceneAsync(sceneName);
            if (operation == null) yield break;

            operation.allowSceneActivation = false;

            while (!operation.isDone)
            {
                var progress = Mathf.Clamp01(operation.progress / 0.9f);
                loadingBar.value = progress;
                progressText.text = "Loading... " + (progress * 100).ToString("F0") + "%";

                if (operation.progress >= 0.9f)
                {
                    yield return new WaitForSeconds(1); // optional

                    // Fade in to a black screen before activating the new scene
                    StartCoroutine(FadeInBeforeActivation(operation));
                    break;
                }

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