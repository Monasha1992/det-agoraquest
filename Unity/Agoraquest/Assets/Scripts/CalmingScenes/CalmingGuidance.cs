using System.Collections;
using UnityEngine;

namespace CalmingScenes
{
    public class CalmingGuidance : MonoBehaviour
    {
        public AudioSource audioSource;
        public AudioClip[] audioClips;
        public AudioClip backgroundClip;

        private void Start()
        {
            StartCoroutine(PlayAudioClipsSequentially());
            //audioSource.PlayOneShot(backgroundClip, 0.3f);
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