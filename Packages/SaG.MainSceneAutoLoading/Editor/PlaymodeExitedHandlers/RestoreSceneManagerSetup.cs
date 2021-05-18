using UnityEditor.SceneManagement;

namespace SaG.MainSceneAutoLoading.PlaymodeExitedHandlers
{
    public class RestoreSceneManagerSetup : IPlaymodeExitedHandler
    {
        public void OnPlaymodeExited(LoadMainSceneArgs args)
        {
            EditorSceneManager.RestoreSceneManagerSetup(args.SceneSetups);
        }
    }
}