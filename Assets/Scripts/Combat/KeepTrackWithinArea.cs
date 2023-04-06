using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class KeepTrackWithinArea<T> : MonoBehaviour where T : Component {
    [Header("Note: Collider needs to be a trigger.")]
    List<T> _inRange;

    private void Awake() {
        _inRange = new List<T>();
    }

    public bool HasAny() { return _inRange.Count > 0; }
    public bool HasAny(out T pick) {
        pick = _inRange.GetRandom();
        return HasAny();
    }

    public bool Contains(T obj) { return _inRange.Contains(obj); }

    void OnTriggerEnter(Collider other) {
        if(other.TryGetComponent<T>(out var entered)) {
            _inRange.AddUnique(entered);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.TryGetComponent<T>(out var exited)) {
            _inRange.Remove(exited);
        }
    }
}