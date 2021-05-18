using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace SaG.MainSceneAutoLoading
{
    public class LoadMainSceneArgs
    {
        public readonly SceneSetup[] SceneSetups;
        public readonly GlobalObjectId[] SelectedObjectsInHierarchy;
        public readonly GlobalObjectId[] UnfoldedObjects;
        public readonly List<string> ExpandedScenes;
        public readonly bool RestoreHierarchyState;

        public LoadMainSceneArgs(
            SceneSetup[] sceneSetups,
            GlobalObjectId[] selectedObjectsInHierarchy,
            GlobalObjectId[] unfoldedObjects,
            List<string> expandedScenes,
            bool restoreHierarchyState)
        {
            SceneSetups = sceneSetups;
            SelectedObjectsInHierarchy = selectedObjectsInHierarchy;
            UnfoldedObjects = unfoldedObjects;
            ExpandedScenes = expandedScenes;
            RestoreHierarchyState = restoreHierarchyState;
        }

        // todo Serialization to save in EditorPrefs
    }
}