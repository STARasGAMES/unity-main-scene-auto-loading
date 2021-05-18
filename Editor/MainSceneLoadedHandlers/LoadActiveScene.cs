using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SaG.MainSceneAutoLoading.MainSceneLoadedHandlers
{
    public class LoadActiveScene : IMainSceneLoadedHandler
    {
        public void OnMainSceneLoaded(LoadMainSceneArgs args)
        {
            var path = args.SceneSetups.First(scene => scene.isActive).path;
            SceneManager.LoadScene(path);
            SceneHierarchyStateUtility.RestoreHierarchyState(args);
        }
        
    }
}