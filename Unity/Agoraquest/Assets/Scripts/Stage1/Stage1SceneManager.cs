using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Stage1
{
    public class Stage1SceneManager : MonoBehaviour
    {
        public Image fadePlane;
        private const float FadeDuration = 5.0f;

        private void Start()
        {
            fadePlane.gameObject.SetActive(true);
            StartCoroutine(FadeInPlane());
        }

        /**
         * for better page transition effect,
         * black plane will be faded in before the scene is loaded
         *
         * will run for the duration of FadeDuration seconds
         * by reducing the alpha value of the plane in every frame
         */
        private IEnumerator FadeInPlane()
        {
            yield return new WaitForSeconds(1);

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