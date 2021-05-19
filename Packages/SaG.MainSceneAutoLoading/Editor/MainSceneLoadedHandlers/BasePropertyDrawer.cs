using UnityEditor;
using UnityEngine;

namespace SaG.MainSceneAutoLoading.MainSceneLoadedHandlers
{
    public abstract class BasePropertyDrawer : PropertyDrawer
    {
        public abstract string Description { get; }
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.enabled = false;
            EditorGUI.indentLevel++;
            EditorGUI.LabelField(position, new GUIContent(Description, Description));
            EditorGUI.indentLevel--;
            GUI.enabled = true;
        }
    }
}