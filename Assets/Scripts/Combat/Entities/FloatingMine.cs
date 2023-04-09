using System.Collections;
using System.Globalization;
using UnityEngine;

[RequireComponent(typeof(ShipEntity))]
[RequireComponent(typeof(Rigidbody))]
public class FloatingMine : ZoneOfEffect<Entity> {
    [SerializeField] float _seekSpeed;

    [SerializeField] float _detonateRadius;
    [SerializeField] float _explosionRadius;
    [SerializeField] float _explosionDamage;
    [SerializeField] float _explosionForce;

    Rigidbody _rbody;

    protected override void Awake() {
        base.Awake();
        // Square the detonate radius value as to compare with Vector3's .sqrMagnitude, which is faster than .magnitude
        _detonateRadius = _detonateRadius * _detonateRadius;
        _rbody = GetComponent<Rigidbody>();
    }

    protected override void FixedUpdate() {
        base.FixedUpdate();
        if (_InRange.Count <= 0) { return; }

        // Get average of all other ships postions in range.
        Vector3 seekPosition = Vector3.zero;
        Vector3 thisPosition = this.transform.position;

        foreach (var ship in _InRange) {
            Vector3 shipPosition = ship.transform.position;

            // If witin detonate radius, don't care about moving anymore.
            if ((shipPosition - thisPosition).sqrMagnitude <= _detonateRadius) {
                Detonate();
                return;
            }
            seekPosition += shipPosition;
        }
        seekPosition /= _InRange.Count;

        // Move towards this position.
        Vector3 delta = seekPosition - thisPosition;
        _rbody.velocity += delta * _seekSpeed * Time.fixedDeltaTime;
    }

    void Detonate() {
        Utility.ForEachComponent<Entity>(
            Physics.OverlapSphere(this.transform.position, _explosionRadius),
            // For each ship, create and send an attack object
            (entity) => {
                Vector3 explosionForce = (entity.transform.position - this.transform.position).normalized * _explosionForce;
                Attack explosionHit = new Attack(_explosionDamage, null, explosionForce);
                explosionHit.OnHit += (atk, entity) => Debug.Log($"{nameof(FloatingMine)} hit {entity.name} for {atk.Damage} damage!"); // this line can be removed
                entity.Hit(explosionHit);
            }
        );

        // add code for explosion fx here
        Destroy(this.gameObject);
    }
}