using UnityEngine;
using Utils;

namespace InitialScene
{
    public class LoadSceneWithProgressBar : MonoBehaviour
    {
        private const string Stage1 = "Stage1";

        public void ToStage1()
        {
            FindFirstObjectByType<SceneLoadManager>().LoadScene(Stage1);
        }
    }
}