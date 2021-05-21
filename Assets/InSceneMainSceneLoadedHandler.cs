#if UNITY_EDITOR // this script should not be present in builds
using System.Collections;
using SaG.MainSceneAutoLoading;
using SaG.MainSceneAutoLoading.MainSceneLoadedHandlers;
using SaG.MainSceneAutoLoading.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InSceneMainSceneLoadedHandler : MonoBehaviour, IMainSceneLoadedHandler
{
    public void OnMainSceneLoaded(LoadMainSceneArgs args)
    {
        Debug.Log($"OnMainSceneLoaded! Now decide what to do with args.SceneSetups...");
        StartCoroutine(LoadDesiredScenes(args));
    }

    IEnumerator LoadDesiredScenes(LoadMainSceneArgs args)
    {
        yield return new WaitForSeconds(1f);
        foreach (var sceneSetup in args.SceneSetups)
        {
            SceneManager.LoadScene(sceneSetup.path, LoadSceneMode.Additive);
        }

        // call this to restore previously selected and expanded GameObjects 
        SceneHierarchyStateUtility.StartRestoreHierarchyStateCoroutine(args);
    }
}
#endif