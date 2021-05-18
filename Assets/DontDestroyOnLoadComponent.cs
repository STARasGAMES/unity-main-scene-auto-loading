using UnityEngine;

public class DontDestroyOnLoadComponent : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
