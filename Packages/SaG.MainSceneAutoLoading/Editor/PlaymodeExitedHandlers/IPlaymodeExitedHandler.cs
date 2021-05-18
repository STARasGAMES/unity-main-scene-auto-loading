namespace SaG.MainSceneAutoLoading.PlaymodeExitedHandlers
{
    public interface IPlaymodeExitedHandler
    {
        void OnPlaymodeExited(LoadMainSceneArgs args);
    }
}