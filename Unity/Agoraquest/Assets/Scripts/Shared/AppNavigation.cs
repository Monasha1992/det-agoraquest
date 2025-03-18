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

        public static void ToCalmingScene(int calmingSceneId)
        {
            var sceneName = $"CalmingScene{calmingSceneId}";
            FindFirstObjectByType<SceneLoadManager>().LoadScene(sceneName);
        }
    }
}