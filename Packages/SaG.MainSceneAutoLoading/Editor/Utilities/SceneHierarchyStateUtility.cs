﻿using System.Collections;
using System.Collections.Generic;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;

namespace SaG.MainSceneAutoLoading
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
            
            SceneHierarchyUtility.SetScenesExpanded(args.ExpandedScenes);
            
            var ids = args.SelectedObjectsInHierarchy;
            List<GameObject> selection = new List<GameObject>(ids.Length);
            foreach (var id in ids)
            {
                var obj = GlobalObjectId.GlobalObjectIdentifierToObjectSlow(id) as Transform;
                if (obj != null)
                {
                    selection.Add(obj.gameObject);
                }
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