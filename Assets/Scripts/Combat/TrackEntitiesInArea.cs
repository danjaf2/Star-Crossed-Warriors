using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TrackEntitiesInArea : MonoBehaviour {
    [Header("Note: Collider needs to be a trigger.")]
    List<Entity> _inRange;

    private void Awake() {
        _inRange = new List<Entity>();
    }

    public bool HasAny() { return _inRange.Count > 0; }
    public bool HasAny(out Entity pick) {
        pick = _inRange.GetRandom();
        return HasAny();
    }

    public bool Contains(Entity obj) { return _inRange.Contains(obj); }

    void OnTriggerEnter(Collider other) {
        if(other.TryGetComponent<Entity>(out var entered)) {
            _inRange.AddUnique(entered);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.TryGetComponent<Entity>(out var exited)) {
            _inRange.Remove(exited);
        }
    }
}