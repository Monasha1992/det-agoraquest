using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Stage1
{
    public class Stage1SceneManager : MonoBehaviour
    {
        public Image fadePlane;
        private const float FadeDuration = 5f;

        private void Start()
        {
            StartCoroutine(FadeIn());
        }

        private IEnumerator FadeIn()
        {
            var t = FadeDuration;
            while (t > 0)
            {
                t -= Time.deltaTime;
                var color = fadePlane.color;
                color.a = t / FadeDuration;
                fadePlane.color = color;
                yield return null;
            }
        }
    }
}