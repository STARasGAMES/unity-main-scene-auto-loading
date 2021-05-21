using System.Linq;
using SaG.MainSceneAutoLoading.Utilities;
using UnityEditor;
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
            
            SceneHierarchyStateUtility.StartRestoreHierarchyStateCoroutine(args);
        }
        
        [CustomPropertyDrawer(typeof(LoadAllLoadedScenes))]
        public sealed class Drawer : BasePropertyDrawer
        {
            public override string Description =>
                $"Loads all scene that was loaded in hierarchy before entering playmode.";
        }
    }
}