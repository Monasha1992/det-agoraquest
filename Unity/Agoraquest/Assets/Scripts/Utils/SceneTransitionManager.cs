using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Utils
{
    public class SceneTransitionManager : MonoBehaviour
    {
        public Image fadeImage;
        public float fadeDuration = 1.0f;

        private void Start()
        {
            StartCoroutine(FadeIn()); // Start with a fade-in effect
        }

        public void LoadSceneWithFade(string sceneName)
        {
            StartCoroutine(FadeOut(sceneName));
        }

        private IEnumerator FadeIn()
        {
            var t = fadeDuration;
            while (t > 0)
            {
                t -= Time.deltaTime;
                var color = fadeImage.color;
                color.a = t / fadeDuration;
                fadeImage.color = color;
                yield return null;
            }
        }

        private IEnumerator FadeOut(string sceneName)
        {
            float t = 0;
            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                var color = fadeImage.color;
                color.a = t / fadeDuration;
                fadeImage.color = color;
                yield return null;
            }

            SceneManager.LoadScene(sceneName);
        }
    }
}