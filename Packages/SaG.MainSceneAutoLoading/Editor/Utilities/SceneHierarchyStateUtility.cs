using System.Collections;
using System.Collections.Generic;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;

namespace SaG.MainSceneAutoLoading.Utilities
{
    public static class SceneHierarchyStateUtility
    {
        public static EditorCoroutine RestoreHierarchyState(LoadMainSceneArgs args)
        {
            return EditorCoroutineUtility.StartCoroutineOwnerless(RestoreHierarchyStateEnumerator(args));
        }

        public static IEnumerator RestoreHierarchyStateEnumerator(LoadMainSceneArgs args)
        {
            yield return null;
            
            // todo: check if at least one scene was loaded
            SceneHierarchyUtility.SetScenesExpanded(args.ExpandedScenes);
            
            var ids = args.SelectedObjectsInHierarchy;
            List<GameObject> selection = new List<GameObject>(ids.Length);
            foreach (var id in ids)
            {
                var obj = GlobalObjectId.GlobalObjectIdentifierToObjectSlow(id) as GameObject;
                // todo: check asset id to identify prefab
                if (obj == null)
                {
                    Debug.LogError($"Can't select object by GlobalObjectId. Most likely it's a prefab. For now, prefab selection do not persist in playmode.\n{id}");
                    continue;
                }
                selection.Add(obj);
            }
            Selection.objects = selection.ToArray();
            
            foreach (var id in args.UnfoldedObjects)
            {
                var obj = GlobalObjectId.GlobalObjectIdentifierToObjectSlow(id);
                if (obj == null || !(obj is GameObject))
                    continue;
                SceneHierarchyUtility.SetExpanded(obj as GameObject, true);
            }
            
        }
    }
}