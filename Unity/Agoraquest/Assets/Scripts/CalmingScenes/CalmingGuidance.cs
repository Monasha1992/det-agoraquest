using System.Collections;
using Shared;
using UnityEngine;

namespace CalmingScenes
{
    public class CalmingGuidance : MonoBehaviour
    {
        public AudioSource audioSource;
        public AudioClip[] audioClips;
        public AudioClip calibrationAudio;
        public AudioClip backgroundClip;

        private void Start()
        {
            if (AppStateData.HasHeartRateCalibrated)
            {
                StartGuidance();
            }
            else
            {
                audioSource.PlayOneShot(backgroundClip, 0.3f);
            }
        }

        public void StartCalibration()
        {
            // stop the background music
            audioSource.Stop();
            StartCoroutine(PlayCalibration());
        }

        private IEnumerator PlayCalibration()
        {
            audioSource.clip = calibrationAudio;
            audioSource.Play();
            yield return new WaitForSeconds(calibrationAudio.length + 3);
        }

        public void StartGuidance()
        {
            // stop the background music
            //audioSource.Stop();
            StartCoroutine(PlayAudioClipsSequentially());
        }

        private IEnumerator PlayAudioClipsSequentially()
        {
            foreach (var clip in audioClips)
            {
                audioSource.clip = clip;
                audioSource.Play();
                yield return new WaitForSeconds(clip.length + 3);
            }
        }
    }
}