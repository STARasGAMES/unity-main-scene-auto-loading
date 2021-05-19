using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

namespace SaG.MainSceneAutoLoading.Utilities
{
    internal static class ObjectUtility
    {
        public static IEnumerable<T> FindInterfacesOfType<T>(bool includeInactive = false)
        {
            return SceneManager.GetActiveScene().GetRootGameObjects()
                .SelectMany(go => go.GetComponentsInChildren<T>(includeInactive));
        }
    }
}