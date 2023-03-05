using UnityEngine;

/// <summary>
/// Singleton that will instantiate itself (if it doesn't already exist) when required.
/// </summary>
public abstract class LazyLoadSingleton<T> : Singleton<T> where T : Component 
{
    private static T _instance;
    public static new T Instance {
        get {
            if (_instance == null) {
                GenerateInstance();
            }
            return _instance;
        }

        protected set {
            if (_instance != null) {
                Debug.LogWarning($"Overriding singleton of type '{typeof(T).Name}'");
            }
            _instance = value;
        }
    }

    private static void GenerateInstance() {
        if (_instance == null) {
            Debug.LogWarning("Generating singleton instance");
            GameObject obj = new GameObject();
            obj.name = typeof(T).Name;
            obj.AddComponent<T>();
        }
    }
}