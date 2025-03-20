using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Levels
{
    public class ColorFlickerScript : MonoBehaviour
    {
        private Image _image;

        private void Start()
        {
            _image = GetComponent<Image>();
            StartCoroutine(StartFlickering());
        }

        private IEnumerator StartFlickering()
        {
            while (true)
            {
                var color = _image.color;
                var targetAlpha = Random.Range(0.1f, 0.2f);
                var elapsedTime = 0f;
                const float duration = 0.1f;

                while (elapsedTime < duration)
                {
                    color.a = Mathf.Lerp(color.a, targetAlpha, elapsedTime / duration);
                    _image.color = color;
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}