using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SaG.MainSceneAutoLoading.Utilities
{
    public static class SceneHierarchyStateUtility
    {
        /// <summary>
        /// Starts EditorCoroutine that will restore previously selected and expanded GameObjects in the hierarchy.
        /// Waits for scenes to load first.
        /// </summary>
        public static EditorCoroutine StartRestoreHierarchyStateCoroutine(LoadMainSceneArgs args)
        {
            var playmodeState = Application.isPlaying;
            return EditorCoroutineUtility.StartCoroutineOwnerless(RestoreHierarchyStateEnumerator(args, playmodeState));
        }

        private static IEnumerator RestoreHierarchyStateEnumerator(LoadMainSceneArgs args, bool playmodeState)
        {
            while (!IsAnySceneLoaded(args.SceneSetups))
            {
                yield return null;
                if (Application.isPlaying != playmodeState)
                {
                    Debug.Log("Playmode state was changed, stopped hierarchy state restore.");
                    yield break;
                }
            }

            yield return null;

            RestoreHierarchyStateImmediate(args);
        }

        private static bool IsAnySceneLoaded(SceneSetup[] sceneSetups)
        {
            return sceneSetups
                .Any(s => SceneManager.GetSceneByPath(s.path).isLoaded);
        }

        /// <summary>
        /// Immediately tries to restore previously selected and expanded GameObjects. If no scene was loaded will log error and return.
        /// </summary>
        /// <param name="args">LoadMainSceneArgs</param>
        public static void RestoreHierarchyStateImmediate(LoadMainSceneArgs args)
        {
            if (!IsAnySceneLoaded(args.SceneSetups))
            {
                Debug.LogError(
                    "Cannot restore hierarchy state because no scene was loaded when this method was called.");
                return;
            }

            SceneHierarchyUtility.SetScenesExpanded(args.ExpandedScenes);

            var ids = args.SelectedInHierarchyObjects;
            List<GameObject> selection = new List<GameObject>(ids.Length);
            bool isMissingObjects = false;
            for (var i = 0; i < ids.Length; i++)
            {
                var id = ids[i];
                var isPrefab = id.targetPrefabId != 0;
                if (isPrefab && Application.isPlaying)
                {
                    id = ConvertPrefabGidToUnpackedGid(id);
                }

                var obj = GlobalObjectId.GlobalObjectIdentifierToObjectSlow(id) as GameObject;
                if (obj == null)
                {
                    isMissingObjects = true;
                    continue;
                }

                selection.Add(obj);
            }

            Selection.objects = selection.ToArray();

            for (var i = 0; i < args.ExpandedInHierarchyObjects.Length; i++)
            {
                var id = args.ExpandedInHierarchyObjects[i];
                var isPrefab = id.targetPrefabId != 0;
                if (isPrefab && Application.isPlaying)
                {
                    id = ConvertPrefabGidToUnpackedGid(id);
                }

                var obj = GlobalObjectId.GlobalObjectIdentifierToObjectSlow(id);
                if (obj == null || !(obj is GameObject))
                {
                    isMissingObjects = true;
                    continue;
                }

                SceneHierarchyUtility.SetExpanded(obj as GameObject, true);
            }

            if (isMissingObjects)
            {
                Debug.LogError("Some selected or expanded objects are missing. Most likely they are destroyed during Awake.");
            }
        }

        // how could I know this by myself... https://uninomicon.com/globalobjectid
        private static GlobalObjectId ConvertPrefabGidToUnpackedGid(GlobalObjectId id)
        {
            ulong fileId = (id.targetObjectId ^ id.targetPrefabId) & 0x7fffffffffffffff;
            bool success = GlobalObjectId.TryParse(
                $"GlobalObjectId_V1-{id.identifierType}-{id.assetGUID}-{fileId}-0",
                out GlobalObjectId unpackedGid);
            return unpackedGid;
        }
    }
}