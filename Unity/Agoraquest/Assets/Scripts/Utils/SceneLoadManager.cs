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

        public void LoadScene(string sceneName)
        {
            StartCoroutine(LoadSceneAsync(sceneName));
        }

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
                    progressText.text = "Press any key to continue...";
                    if (Input.anyKeyDown)
                        operation.allowSceneActivation = true;
                }

                yield return null;
            }
        }
    }
}