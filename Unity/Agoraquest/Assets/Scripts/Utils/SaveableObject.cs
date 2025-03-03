using System.Linq;
using UnityEngine;
using System.Collections.Generic;

namespace Utils
{

    public class SaveableObject : MonoBehaviour, ISaveable
    {

        [SerializeField]
        private string uniqueID;

        private static readonly Dictionary<string, SaveableObject> idRegistry = new Dictionary<string, SaveableObject>();

        public string GetUniqueID() => uniqueID;

        public ObjectData SaveData()
        {
            return new ObjectData
            {
                id = uniqueID,
                position = transform.position,
                scale = transform.localScale,
                rotation = transform.rotation,
                isActive = gameObject.activeSelf
            };
        }

        public void LoadData(ObjectData data)
        {
            transform.position = data.position;
            transform.localScale = data.scale;
            transform.rotation = data.rotation;
            gameObject.SetActive(data.isActive);
        }



#if UNITY_EDITOR
        [ContextMenu("Generate unique ID")]
        private void GenerateGUID()
        {
            string newID;
            int safetyCouter = 0;

            do
            {
                newID = System.Guid.NewGuid().ToString();
                safetyCouter++;

                //avoid infinite loop
                if(safetyCouter > 100)
                {
                    Debug.LogError("Failed to generate ID");
                    return;
                }
            }
            while (IsIDExists(newID));

            uniqueID = newID;
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(gameObject.scene);
        }

        private bool IsIDExists(string testID)
        {
            return FindObjectsByType<SaveableObject>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None)
                .Any(obj => obj != this && obj.uniqueID == testID);
        }
#endif


    void Awake()
        {
            //register ID
            if (!string.IsNullOrEmpty(uniqueID))
            {
                if (idRegistry.ContainsKey(uniqueID))
                {
                    Debug.LogError($"ID conflict: {uniqueID} in {name} conflict with {idRegistry[uniqueID].name}");
                    GenerateRuntimeID();
                }
                else
                {
                    idRegistry.Add(uniqueID, this);
                }

            }
            else
            {
                Debug.LogError($"Object {name} lack of uniqueID !!", this);
            }
        }

        void OnDestroy()
        {
            if (!string.IsNullOrEmpty(uniqueID) && idRegistry.ContainsKey(uniqueID))
            {
                idRegistry.Remove(uniqueID);
            }
        }

        //for Dynamic generate ID
        public void GenerateRuntimeID()
        {
            string newID;
            do
            {
                newID = $"DYNAMIC_{System.Guid.NewGuid()}";
            }
            while (idRegistry.ContainsKey(newID));

            //remove old ID
            if(!string.IsNullOrEmpty(uniqueID) && idRegistry.ContainsKey(uniqueID) )
            {
                idRegistry.Remove(uniqueID);
            }

            uniqueID = newID;
            idRegistry.Add(uniqueID, this);
        }
    }
}

