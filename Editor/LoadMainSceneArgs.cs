using UnityEditor.SceneManagement;

namespace SaG.MainSceneAutoLoading
{
    public class LoadMainSceneArgs
    {
        public readonly SceneSetup[] SceneSetups;

        public LoadMainSceneArgs(SceneSetup[] sceneSetups)
        {
            SceneSetups = sceneSetups;
        }
    }
}