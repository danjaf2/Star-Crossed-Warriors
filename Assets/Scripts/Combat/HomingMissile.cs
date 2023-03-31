using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class HomingMissile : Entity {

    [Header("Navigation")]
    [SerializeField] float _turnSpeed;
    [SerializeField] float _speed;

    [Header("Gameplay")]
    [SerializeField] float _blastRadius;
    [SerializeField] float _damage;
    [SerializeField] int _timer;

    Entity _target;
    Entity _sender;
    bool _detonated;

    //Vector3 _velocity;
    Rigidbody _rbody;

    const float AIR_FRICTION = 0.95f;

    protected override void Awake() {
        base.Awake();
        _rbody = GetComponent<Rigidbody>();
        _speed *= Time.fixedDeltaTime;
        _turnSpeed *= Time.fixedDeltaTime;
    }

    #region FOR DEBUG ONLY
    private void Update() {
        if(Input.GetKeyDown(KeyCode.G)) {
            _target = GameObject.Find("missile_test_target").GetComponent<Entity>();
        }
    }
    #endregion

    protected override void FixedUpdate() {
        base.FixedUpdate();

        if (_timer-- <= 0) { OnDeath(); }
        if (_target == null) { return; }

        Vector3 delta = (_target.transform.position - this.transform.position);
        this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, Quaternion.LookRotation(delta), _turnSpeed);

        // Rotate and accelerate towards target.
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
        if(_detonated) { return; }
        base.OnDeath();

        _detonated = true;
        Utility.ForEachComponent<Entity>(
            Physics.OverlapSphere(this.transform.position, _blastRadius),
            (entity) => {
                if(entity == this) { return; }
                entity.Hit(new Attack(_damage, _sender));
            }
        );
    }

    public static HomingMissile CreateFromPrefab(HomingMissile prefab, Vector3 position, Quaternion rotation, Entity sender, Entity target) {
        HomingMissile missile = Instantiate(prefab, position, rotation);
        missile._sender = sender;
        missile._target = target;
        return missile;
    }
}