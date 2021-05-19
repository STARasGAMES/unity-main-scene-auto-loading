using SaG.MainSceneAutoLoading;
using SaG.MainSceneAutoLoading.MainSceneLoadedHandlers;
using UnityEngine;

public class InSceneMainSceneLoadedHandler : MonoBehaviour, IMainSceneLoadedHandler
{
    public void OnMainSceneLoaded(LoadMainSceneArgs args)
    {
        Debug.Log($"[InSceneMainSceneLoadedHandler] OnMainSceneLoaded");
    }
}
