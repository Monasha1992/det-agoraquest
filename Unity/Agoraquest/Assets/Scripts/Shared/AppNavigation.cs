using UnityEngine;
using Utils;

namespace Shared
{
    public class AppNavigation : MonoBehaviour
    {
        public static void ToStage(int stageId)
        {
            var sceneName = $"Stage{stageId}";
            FindFirstObjectByType<SceneLoadManager>().LoadScene(sceneName);
        }

        public static void ToCalmingScene()
        {
            var calmingSceneId = 1;
            var sceneName = $"CalmingScene{calmingSceneId}";
            FindFirstObjectByType<SceneLoadManager>().LoadScene(sceneName);
        }
    }
}