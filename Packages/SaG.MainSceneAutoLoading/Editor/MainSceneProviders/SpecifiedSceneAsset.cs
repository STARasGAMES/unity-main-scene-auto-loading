using System;
using UnityEditor;
using UnityEngine;

namespace SaG.MainSceneAutoLoading.MainSceneProviders
{
    [Serializable]
    public class SpecifiedSceneAsset : IMainSceneProvider
    {
        [SerializeField]
        private SceneAsset _sceneAsset;

        public SpecifiedSceneAsset()
        {
            _sceneAsset = null;
        }
        
        public SpecifiedSceneAsset(SceneAsset sceneAsset)
        {
            _sceneAsset = sceneAsset;
        }

        public SceneAsset Get()
        {
            return _sceneAsset;
        }
    }
    
    [CustomPropertyDrawer(typeof(SpecifiedSceneAsset), true)]
    internal class ConcreteMainSceneProviderPropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 9 * 2;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            EditorGUI.indentLevel ++;

            EditorGUI.PropertyField(position, property.FindPropertyRelative("_sceneAsset"));

            EditorGUI.indentLevel --;

            EditorGUI.EndProperty();
        }
    }
}