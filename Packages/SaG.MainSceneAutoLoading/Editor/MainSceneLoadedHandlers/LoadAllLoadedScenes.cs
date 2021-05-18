﻿using System.Linq;
using UnityEngine.SceneManagement;

namespace SaG.MainSceneAutoLoading.MainSceneLoadedHandlers
{
    public class LoadAllLoadedScenes : IMainSceneLoadedHandler
    {
        public void OnMainSceneLoaded(LoadMainSceneArgs args)
        {
            SceneManager.LoadScene(args.SceneSetups.First(s => s.isActive).path);
            foreach (var sceneSetup in args.SceneSetups.Where(scene => scene.isLoaded && !scene.isActive))
            {
                SceneManager.LoadScene(sceneSetup.path, LoadSceneMode.Additive);
            }
            
            SceneHierarchyStateUtility.RestoreHierarchyState(args);
        }
    }
}