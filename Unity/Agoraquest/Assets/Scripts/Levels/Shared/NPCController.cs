using System.Collections;
using System.IO;
using System.Text;
using Models;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

namespace Levels.Shared
{
    public class NpcController : MonoBehaviour
    {
        private ConversationsManager _conversationsManager;
        private NavMeshAgent _agent;
        private Animator _animator;
        private AudioSource _audioSource;
        private bool _inConversation;

        private void Start()
        {
            _conversationsManager = FindAnyObjectByType<ConversationsManager>();
            _agent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            _audioSource = GetComponent<AudioSource>();

            AssignToGroupAndMove();
        }


        private void AssignToGroupAndMove()
        {
            var conversation = _conversationsManager.AssignToConversation(this);
            WalkTo(conversation.Position);
        }

        private void WalkTo(Vector3 target)
        {
            _agent.SetDestination(target);
            // animator.SetTrigger("Walk");
            StartCoroutine(CheckArrival(target));
        }

        private IEnumerator CheckArrival(Vector3 target)
        {
            while (Vector3.Distance(transform.position, target) > 1.5f)
            {
                yield return null;
            }

            _conversationsManager.HasArrived(this);
        }

        public IEnumerator Speak(string message)
        {
            // TODO: play message
            // animator.SetTrigger("Talk");
            // Debug.Log(message);
            yield return StartCoroutine(GetSpeechAudio(message, isMale: true));
        }

        private IEnumerator GetSpeechAudio(string text, bool isMale)
        {
            const string apiKey = "AIzaSyBjkfKNrhVrBMcNgDhvNTUE3r2nCrpKNxc";
            const string url = "https://texttospeech.googleapis.com/v1/text:synthesize?key=" + apiKey;

            // Prepare JSON payload
            var json = "{\"input\":{\"text\":\"" + text +
                       "\"},\"voice\":{\"languageCode\":\"en-US\",\"name\":\"en-US-Wavenet-D\",\"ssmlGender\":\"" +
                       (isMale ? "MALE" : "FEMALE") + "\"}," +
                       "\"audioConfig\":{\"audioEncoding\":\"MP3\"}}";

            using (var request = UnityWebRequest.PostWwwForm(url, json))
            {
                request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    var responseJson = request.downloadHandler.text;
                    var response = JsonUtility.FromJson<SpeechResponse>(responseJson);
                    var audioBytes = System.Convert.FromBase64String(response.audioContent);
                    StartCoroutine(PlayAudio(audioBytes));
                    while (_audioSource.isPlaying)
                    {
                        yield return null;
                    }
                }
                else
                {
                    Debug.LogError("TTS Request Failed: " + request.error);
                }
            }
        }

        private IEnumerator PlayAudio(byte[] audioData)
        {
            var tempFilePath = Path.Combine(Application.persistentDataPath, "speech.mp3");
            File.WriteAllBytes(tempFilePath, audioData);

            yield return StartCoroutine(PlayAudioFromFile(tempFilePath));
        }

        private IEnumerator PlayAudioFromFile(string filePath)
        {
            using (var audioRequest = UnityWebRequestMultimedia.GetAudioClip("file://" + filePath, AudioType.MPEG))
            {
                yield return audioRequest.SendWebRequest();

                if (audioRequest.result == UnityWebRequest.Result.Success)
                {
                    var clip = DownloadHandlerAudioClip.GetContent(audioRequest);
                    _audioSource.clip = clip;
                    _audioSource.Play();
                }
                else
                {
                    Debug.LogError("Failed to load audio: " + audioRequest.error);
                }
            }
        }
    }
}