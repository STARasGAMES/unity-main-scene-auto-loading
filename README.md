# Main Scene Auto Loading
Main, initial, loader, boot, bootstrap - you name it.

Whenever you enter playmode this tool firstly loads the main scene, and only after that loads desired scene(s). 

![image](https://user-images.githubusercontent.com/23558898/118852487-81cd5c00-b8db-11eb-8c40-de2e1ae2a458.png)


https://user-images.githubusercontent.com/23558898/118852692-ab868300-b8db-11eb-9f79-9c9e76c96f76.mp4


https://user-images.githubusercontent.com/23558898/118852714-ae817380-b8db-11eb-8217-cbb1f810175e.mp4



For more details why you need this and how to implement it: https://forum.unity.com/threads/executing-first-scene-in-build-settings-when-pressing-play-button-in-editor.157502/

Features:
 + Persist hierarchy state(selected and expanded objects) on entering/exiting playmode.
 + Convinient settings UI located in ProjectSettings window.
 + UPM support, simple install.
 + Highly extendable for any project.

Dependencies:
 + Unity 2019.4+ (uses SerializeReference attribute, [docs](https://docs.unity3d.com/2019.3/Documentation/ScriptReference/SerializeReference.html))
 + EditorCoroutines package, [docs](https://docs.unity3d.com/Packages/com.unity.editorcoroutines@1.0/manual/index.html)

Known issues:
 + Hierarchy state and selections are lost on domain reload (when exiting playmode after script compilation)
 + Prefab selection missed after scene load

## Installation
Install via git url by adding this entry in your **manifest.json**

`"com.sag.main-scene-auto-loading": "https://github.com/STARasGAMES/unity-main-scene-auto-loading.git#upm"`

Or using PackageManager window:
1. copy this link `https://github.com/STARasGAMES/unity-main-scene-auto-loading.git#upm`,
2. open PackageManager window,
3. click `+` button in top-left corner,
4. select `Add package from git URL`,
5. in appeared field paste the link you copied before (`https://github.com/STARasGAMES/unity-main-scene-auto-loading.git#upm`).

## How it works
After installing this package it will create `Assets/MainSceneAutoLoadingSettings.asset` asset. You can change settings by selecting this asset or by using ProjectSettings window.

`MainSceneAutoLoader` listens for play button click. When this happens it asks `MainSceneProvider` for main scene and sets the `EditorSceneManager.playModeStartScene`([docs](https://docs.unity3d.com/ScriptReference/SceneManagement.EditorSceneManager-playModeStartScene.html)) to received value. After that Unity directly loads specified scene by itself.

When main scene loaded `MainSceneLoadedHandler` receives notification with additional information about preveiously opened scenes, selected and expanded objects. This handler decides what to do with provided arguments. Default handler will load all previously loaded scenes.

`MainSceneAutoLoader` also listens for exiting playmode. It will notify `PlaymodeExitedHandler` when editor fully exited playmode. Default handler will restore previously opened scenes, selected and expanded objects. 


## Extending
For now, there are 3 ways to extend:
 + IMainSceneProvider
 + IMainSceneLoadedHandler
 + IPlaymodeExitedHandler

Main thing to remember - all extended scripts should be in editor assemblies! e.g. under `Editor` folder or within editor .asmdef

### IMainSceneProvider
Responsible for providing the main scene, that will be loaded first when entering playmode. It's quite simple:
```c#
public interface IMainSceneProvider
{
    SceneAsset Get();
}
```

To implement your own realization you just need to derive from this interface. Remember to place this script under Editor folder.
```c#
using SaG.MainSceneAutoLoading.MainSceneProviders;
using UnityEditor;
using UnityEngine;

public class Sample_MainSceneProvider : IMainSceneProvider
{
    [SerializeField]
    private bool _setting1;

    [SerializeField]
    private int _setting2;

    [SerializeField]
    private Object _setting3;

    public SceneAsset Get()
    {
        // implementation
        throw new System.NotImplementedException();
    }
}
```

And after that you could find your option in settings's dropdown:
![image](https://user-images.githubusercontent.com/23558898/118925561-9a735b80-b947-11eb-915e-74811f5f99a9.png)

Also, you could write custom property drawer if you'd like to remove setting's folding and provide additional information:
```c#
[CustomPropertyDrawer(typeof(Sample_MainSceneProvider))]
public class Sample_MainSceneProviderPropertyDrawer : PropertyDrawer
{
    private const int FieldsCount = 4;
    private const int FieldHeightSelf = 18;
    private const int FieldHeightTotal = 20;


    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return FieldHeightTotal * FieldsCount;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.indentLevel++;

        position.height = FieldHeightSelf;
        GUI.enabled = false;
        EditorGUI.LabelField(position, "Custom description");
        GUI.enabled = true;
        position.y += FieldHeightTotal;
        EditorGUI.PropertyField(position, property.FindPropertyRelative("_setting1"));
        position.y += FieldHeightTotal;
        EditorGUI.PropertyField(position, property.FindPropertyRelative("_setting2"));
        position.y += FieldHeightTotal;
        EditorGUI.PropertyField(position, property.FindPropertyRelative("_setting3"));

        EditorGUI.indentLevel--;
    }
}
```
![image](https://user-images.githubusercontent.com/23558898/118938973-e0382000-b957-11eb-8b88-aa9bbfc0de7e.png)

### IMainSceneLoadedHandler

The same as above, just implement this interface and you are good to go.

However, you could also create an in-scnene handler and combine with DelegateToInSceneImplementations option.
```c#
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
        SceneHierarchyStateUtility.RestoreHierarchyState(args);
    }
}
#endif
```

### IPlaymodeExitedHandler

Look at the default implementation:
https://github.com/STARasGAMES/unity-main-scene-auto-loading/blob/main/Packages/SaG.MainSceneAutoLoading/Editor/PlaymodeExitedHandlers/RestoreSceneManagerSetup.cs
