using System.Collections;
using UnityEngine;

public class PayloadMissile : Entity {
    [SerializeField] GameObject _payloadPrefab;
    [SerializeField] float _speed;
    [SerializeField] int _lifetime;

    Rigidbody _rbody;
    const float AIR_FRICTION = 0.95f;

    protected override void Awake() {
        base.Awake();
        _rbody = GetComponent<Rigidbody>();
    }


    protected override void FixedUpdate() {
        base.FixedUpdate();

        if (_lifetime-- <= 0) { OnDeath(); }

        Vector3 velocity = _rbody.velocity;
        // Air friction (results in max speed = 20 * _speed)
        velocity *= AIR_FRICTION;
        velocity += this.transform.forward * _speed;
        _rbody.velocity = velocity;
    }

    private void OnCollisionEnter(Collision collision) {
        OnDeath();
    }

    protected override void OnDeath() {
        Detonate();
        base.OnDeath();
    }

    void Detonate() {
        Instantiate(_payloadPrefab, this.transform.position, this.transform.rotation);
    }


    public static PayloadMissile Create(PayloadMissile missilePrefab, Vector3 position, Vector3 velocity) {
        PayloadMissile newMissile = Instantiate(missilePrefab, position, Quaternion.LookRotation(velocity));
        newMissile._rbody.velocity = velocity;

        return newMissile;
    }
}