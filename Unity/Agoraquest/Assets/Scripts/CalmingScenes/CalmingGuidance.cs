using System.Collections;
using UnityEngine;

namespace CalmingScenes
{
    public class CalmingGuidance : MonoBehaviour
    {
        public AudioSource audioSource;
        public AudioClip[] audioClips;

        private void Start()
        {
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