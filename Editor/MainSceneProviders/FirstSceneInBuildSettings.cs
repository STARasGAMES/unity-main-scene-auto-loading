using System;
using System.Linq;
using SaG.MainSceneAutoLoading.Utilities;
using UnityEditor;
using UnityEngine;

namespace SaG.MainSceneAutoLoading.MainSceneProviders
{
    [Serializable]
    public class FirstSceneInBuildSettings : IMainSceneProvider
    {
        public SceneAsset Get()
        {
            var editorBuildSettingsScene = EditorBuildSettings.scenes.FirstOrDefault(x => x.enabled);
            if (editorBuildSettingsScene == null)
            {
                Debug.LogError("Cannot provide main scene, because there is no enabled scene in build settings.");
                return null;
            }

            var path = editorBuildSettingsScene.path;
            SceneAsset sceneAsset = SceneAssetUtility.ConvertPathToSceneAsset(path);
            return sceneAsset;
        }
    }

    [CustomPropertyDrawer(typeof(FirstSceneInBuildSettings))]
    internal sealed class FirstSceneInBuildSettingsMainSceneProviderPropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 9 * 2;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.indentLevel ++;

            var sceneLabel = new GUIContent("There is no enabled scene in Build Settings!!!");
            SceneAsset mainSceneAsset = null;
            GUI.enabled = false;
            var firstScene = EditorBuildSettings.scenes.FirstOrDefault(s => s.enabled);
            if (firstScene != null)
            {
                sceneLabel = new GUIContent("Scene that will be used as main scene", "Scene that will be used as main scene");
                mainSceneAsset = SceneAssetUtility.ConvertPathToSceneAsset(firstScene.path);
            }

            EditorGUI.ObjectField(position, sceneLabel, mainSceneAsset, typeof(SceneAsset), false);
            GUI.enabled = true;

            EditorGUI.indentLevel --;
        }
    }
}