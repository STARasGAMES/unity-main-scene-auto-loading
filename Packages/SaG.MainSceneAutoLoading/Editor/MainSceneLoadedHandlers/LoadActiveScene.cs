using System.Linq;
using UnityEngine.SceneManagement;

namespace SaG.MainSceneAutoLoading.MainSceneLoadedHandlers
{
    public class LoadActiveScene : IMainSceneLoadedHandler
    {
        public void OnMainSceneLoaded(LoadMainSceneArgs args)
        {
            var path = args.SceneSetups.First(scene => scene.isActive).path;
            SceneManager.LoadScene(path);
        }
    }
}