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

        public LoadMainSceneArgs(
            SceneSetup[] sceneSetups,
            GlobalObjectId[] selectedObjectsInHierarchy,
            GlobalObjectId[] unfoldedObjects,
            List<string> expandedScenes)
        {
            SceneSetups = sceneSetups;
            SelectedObjectsInHierarchy = selectedObjectsInHierarchy;
            UnfoldedObjects = unfoldedObjects;
            ExpandedScenes = expandedScenes;
        }
        
        // todo Serialization to save in EditorPrefs
    }
}