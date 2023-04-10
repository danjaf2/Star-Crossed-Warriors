using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class HomingMissile : Entity {

    [Header("Navigation")]
    [SerializeField] float _turnSpeed;
    [SerializeField] float _speed;

    [Header("Gameplay")]
    [SerializeField] float _blastRadius;
    [SerializeField] int _lifetime;

    Entity _target;
    bool _detonated;

    Rigidbody _rbody;

    Attack _toDeliver;

    const float AIR_FRICTION = 0.95f;

    protected override void Awake() {
        base.Awake();
        _rbody = GetComponent<Rigidbody>();
        _speed *= Time.fixedDeltaTime;
        _turnSpeed *= Time.fixedDeltaTime;
    }

    //#region FOR DEBUG ONLY
    //private void Update() {
    //    if(Input.GetKeyDown(KeyCode.G)) {
    //        _target = GameObject.Find("missile_test_target").GetComponent<Entity>();
    //    }
    //}
    //#endregion

    protected override void FixedUpdate() {
        base.FixedUpdate();

        if (_lifetime-- <= 0) { OnDeath(); }
        if (_target == null) { return; }
        //Debug.Log(_target.name);
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
        if (collision.gameObject.layer != 2)
        {
            //Debug.Log(collision.gameObject.name);
            OnDeath();
        }
    }

    protected override void OnDeath() {
        if(_detonated) { return; }
        base.OnDeath();

        _detonated = true;
        Utility.ForEachComponent<Entity>(
            Physics.OverlapSphere(this.transform.position, _blastRadius),
            (entity) => {
                if(entity == this) { return; }
                // Here, since there may be multiple targets hit, we want to create an attack copy and send one to each.
                // Otherwise, an effect that modifies the attack's values will affect all entities hit.
                entity.Hit(new Attack(_toDeliver));
            }
        );
    }

    public static HomingMissile Create(HomingMissile prefab, Vector3 position, Vector3 initVelocity, Entity target, Attack attack) {
        HomingMissile missile = Instantiate(prefab, position, Quaternion.Euler(initVelocity));
        missile._toDeliver = attack;
        missile._target = target;
        missile._rbody.velocity = initVelocity;
        return missile;
    }
}