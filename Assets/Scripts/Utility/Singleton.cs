using UnityEngine;

/// <summary>
/// Represents a publicly available object that should only have a single instance.
/// </summary>
public abstract class Singleton<T> : MonoBehaviour where T : Component {
    private static T _instance;
    public static T Instance {
        get {
            return _instance;
        }

        protected set {
            if (_instance != null) {
                Debug.LogWarning($"Overriding singleton of type '{typeof(T).Name}'");
                Destroy(_instance);
            }
            _instance = value;
        }
    }

    /// <summary>
    /// Should contain the following code.
    /// <code>Instance = this;</code>
    /// 
    /// This is somewhat unclean, but it lets singletons assign 
    /// themselves without requirng a scene-wide search.
    /// </summary>
    protected abstract void Awake();
}