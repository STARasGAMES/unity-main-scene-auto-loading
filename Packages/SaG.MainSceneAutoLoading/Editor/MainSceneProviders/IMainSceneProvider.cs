using UnityEditor;

namespace SaG.MainSceneAutoLoading.MainSceneProviders
{
    public interface IMainSceneProvider
    {
        SceneAsset Get();
    }
}