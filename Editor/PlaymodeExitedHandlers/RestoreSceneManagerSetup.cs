using SaG.MainSceneAutoLoading.Utilities;

namespace SaG.MainSceneAutoLoading.PlaymodeExitedHandlers
{
    public class RestoreSceneManagerSetup : IPlaymodeExitedHandler
    {
        public void OnPlaymodeExited(LoadMainSceneArgs args)
        {
            // by not calling this we let Unity restore unsaved changes in the scene
            // EditorSceneManager.RestoreSceneManagerSetup(args.SceneSetups);
            SceneHierarchyStateUtility.RestoreHierarchyState(args);
        }
    }
}