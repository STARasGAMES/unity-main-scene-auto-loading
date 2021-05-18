using UnityEditor;

namespace SaG.MainSceneAutoLoading.MainSceneProviders
{
    public class ConcreteMainSceneProvider : IMainSceneProvider
    {
        private readonly SceneAsset _sceneAsset;

        public ConcreteMainSceneProvider(SceneAsset sceneAsset)
        {
            _sceneAsset = sceneAsset;
        }

        public SceneAsset Get()
        {
            return _sceneAsset;
        }
    }
}