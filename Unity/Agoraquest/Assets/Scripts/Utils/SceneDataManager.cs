using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;


namespace Utils
{
    [System.Serializable]
    public class ObjectData
    {
        public string id;
        public Vector3 position;
        public Vector3 scale;
        public Quaternion rotation;
        public bool isActive;

        //add something need to be saved, then update SavaObject.cs
    }

    public interface ISaveable
    {
        string GetUniqueID();
        ObjectData SaveData();
        void LoadData(ObjectData data);
    }

    public class SceneDataManager : MonoBehaviour
    {
        public static SceneDataManager Instance;

        private Dictionary<string, List<ObjectData>> savedSceneData = new Dictionary<string, List<ObjectData>>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                SceneManager.sceneLoaded += OnSceneLoaded;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            LoadSceneData(scene.name);
        }

        public void SaveCurrentSceneData()
        {
            Scene currentScene = SceneManager.GetActiveScene();
            SaveSceneData(currentScene.name);
        }

        private void SaveSceneData(string sceneName)
        {
            List<ObjectData> dataList = new List<ObjectData>();

            foreach (var savable in FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None).OfType<ISaveable>())
            {
                dataList.Add(savable.SaveData());
            }

            savedSceneData[sceneName] = dataList;
            Debug.Log($"Saved {dataList.Count} objects in {sceneName}");
        }

        private void LoadSceneData(string sceneName)
        {
            if(savedSceneData.TryGetValue(sceneName, out List<ObjectData> dataList))
            {
                foreach(var data in dataList)
                {
                    ISaveable saveable = FindSaveableByID(data.id);
                    if(saveable != null)
                    {
                        saveable.LoadData(data);
                    }
                    else
                    {
                        Debug.LogWarning($"Object {data.id} not found in {sceneName}");
                    }
                }
                Debug.Log($"Loaded {dataList.Count} objects in {sceneName}");
            }
        }

        private ISaveable FindSaveableByID(string id)
        {
            return FindObjectsByType<MonoBehaviour>
                    (FindObjectsInactive.Include, FindObjectsSortMode.None)
                    .OfType<ISaveable>()
                    .FirstOrDefault(s => s.GetUniqueID() == id);

        }


        public void ClearData(string sceneName)
        {
            savedSceneData.Remove(sceneName);
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
