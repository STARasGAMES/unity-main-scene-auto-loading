using System.Linq;
using SaG.MainSceneAutoLoading.Utilities;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace SaG.MainSceneAutoLoading.MainSceneLoadedHandlers
{
    public class LoadActiveScene : IMainSceneLoadedHandler
    {
        public void OnMainSceneLoaded(LoadMainSceneArgs args)
        {
            var path = args.SceneSetups.First(scene => scene.isActive).path;
            SceneManager.LoadScene(path);
            SceneHierarchyStateUtility.StartRestoreHierarchyStateCoroutine(args);
        }
        
        [CustomPropertyDrawer(typeof(LoadActiveScene))]
        public sealed class Drawer : BasePropertyDrawer
        {
            public override string Description =>
                $"Loads only one scene that was active(with bold name) in hierarchy before entering playmode.";
        }
    }
}