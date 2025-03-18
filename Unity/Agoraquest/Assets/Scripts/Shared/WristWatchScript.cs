using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Shared
{
    public class WristWatchScript : MonoBehaviour
    {
        public GameObject panicButtonIndicator;
        public TextMeshProUGUI spentTimeText;

        private bool _isInPanic = true;
        private float _elapsedTime = 0f;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            if (panicButtonIndicator != null) StartCoroutine(AnimateActionButton());
            if (spentTimeText != null) StartCoroutine(UpdateSpentTime());
        }

        public void ActivatePanicButton(bool isInPanic)
        {
            _isInPanic = isInPanic;
        }

        private IEnumerator AnimateActionButton()
        {
            var isBig = false;
            while (true)
            {
                if (!_isInPanic) break;

                panicButtonIndicator.transform.localScale = new Vector3(
                    isBig ? 0.02f : 0.022f,
                    0.001f,
                    isBig ? 0.02f : 0.022f
                );
                isBig = !isBig;
                yield return new WaitForSeconds(0.2f);
            }
        }

        private IEnumerator UpdateSpentTime()
        {
            while (true)
            {
                _elapsedTime += Time.deltaTime;
                var timeSpan = TimeSpan.FromSeconds(_elapsedTime);
                spentTimeText.text = $"{timeSpan:mm\\:ss}";

                // TODO: Save the time to the device
                yield return new WaitForSeconds(1f);
            }
        }
    }
}