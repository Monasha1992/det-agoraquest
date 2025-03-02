using TMPro;
using UnityEngine;

namespace Shared
{
    public class WristMonitorManager : MonoBehaviour
    {
        public TextMeshProUGUI heartRateValueText;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            heartRateValueText.text = "0";
        }

        // Update is called once per frame
        private void Update()
        {
        }
    }
}