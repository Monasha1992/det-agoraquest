using System.Collections;
using UnityEngine;

namespace Shared
{
    public class HearIconAnimationScript : MonoBehaviour
    {
        private Transform _transform;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            _transform = GetComponent<Transform>();
            StartCoroutine(ChangeScaleEverySecond());
        }

        // Coroutine to change the scale every second
        private IEnumerator ChangeScaleEverySecond()
        {
            var isBig = false;
            while (true)
            {
                _transform.localScale = new Vector3(
                    isBig ? 0.01f : 0.011f,
                    isBig ? 0.01f : 0.011f,
                    isBig ? 0.01f : 0.011f
                );
                isBig = !isBig;
                yield return new WaitForSeconds(0.2f);
            }
        }
    }
}