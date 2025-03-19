using UnityEngine;
using Utils;

namespace Shared
{
    public class AppNavigation : MonoBehaviour
    {
        public static void ToStage(int stageSceneId)
        {
            var sceneName = $"Level{stageSceneId}Scene";
            FindFirstObjectByType<SceneLoadManager>().LoadScene(sceneName);
        }

        public static void ToCalmingScene(CalmingScene calmingScene)
        {
            var sceneName = $"{calmingScene}CalmingScene";
            FindFirstObjectByType<SceneLoadManager>().LoadScene(sceneName);
        }
    }
}