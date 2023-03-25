using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneOfEffect<T> : MonoBehaviour where T : Component {

    [SerializeField] float _radius;
    protected List<T> _InRange;

    protected virtual void Awake() {
        _InRange = new List<T>();
    }

    //private void OnTriggerEnter(Collider other) {
    //    if(other.TryGetComponent<T>(out var ship)) {
    //        _InRange.AddUnique(ship);
    //    }
    //}

    //private void OnTriggerExit(Collider other) {
    //    if (other.TryGetComponent<T>(out var ship)) {
    //        _InRange.Remove(ship);
    //    }
    //}

    static bool IsNull(T entity) => entity == null;
    void AddToList(T newEntity) {
        if (newEntity.gameObject != this.gameObject) { _InRange.Add(newEntity); }
    }

    protected virtual void FixedUpdate() {
        _InRange.Clear();
        Utility.ForEachComponent<T>(Physics.OverlapSphere(this.transform.position, _radius), AddToList);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}