using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace SaG.MainSceneAutoLoading
{
    public class LoadMainSceneArgs
    {
        public readonly SceneSetup[] SceneSetups;
        public readonly GlobalObjectId[] SelectedInHierarchyObjects;
        public readonly GlobalObjectId[] ExpandedInHierarchyObjects;
        public readonly List<string> ExpandedScenes;

        public LoadMainSceneArgs(
            SceneSetup[] sceneSetups,
            GlobalObjectId[] selectedInHierarchyObjects,
            GlobalObjectId[] expandedInHierarchyObjects,
            List<string> expandedScenes)
        {
            SceneSetups = sceneSetups;
            SelectedInHierarchyObjects = selectedInHierarchyObjects;
            ExpandedInHierarchyObjects = expandedInHierarchyObjects;
            ExpandedScenes = expandedScenes;
        }

        [System.Serializable]
        private class SaveData
        {
            public SceneSetup[] SceneSetups;
            public string[] SelectedInHierarchyObjects;
            public string[] ExpandedInHierarchyObjects;
            public List<string> ExpandedScenes;
        }

        public string Serialize()
        {
            var saveData = new SaveData()
            {
                SceneSetups = SceneSetups,
                SelectedInHierarchyObjects = SelectedInHierarchyObjects.Select(x => x.ToString()).ToArray(),
                ExpandedInHierarchyObjects = ExpandedInHierarchyObjects.Select(x => x.ToString()).ToArray(),
                ExpandedScenes = ExpandedScenes
            };

            var json = JsonUtility.ToJson(saveData);
            return json;
        }

        public static LoadMainSceneArgs Deserialize(string json)
        {
            var saveData = JsonUtility.FromJson<SaveData>(json);
            
            var args = new LoadMainSceneArgs(
                saveData.SceneSetups,
                ParseGlobalObjectIds(saveData.SelectedInHierarchyObjects),
                ParseGlobalObjectIds(saveData.ExpandedInHierarchyObjects),
                saveData.ExpandedScenes
                );

            return args;
        }

        private static GlobalObjectId[] ParseGlobalObjectIds(string[] stringIds)
        {
            var ids = new List<GlobalObjectId>(stringIds.Length);
            foreach (var stringId in stringIds)
            {
                if (GlobalObjectId.TryParse(stringId, out var id))
                {
                    ids.Add(id);
                }
            }

            return ids.ToArray();
        }
        
    }
}