using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.Netcode;
using UnityEngine;

public class DemoShip : PlayerShip {

    //maxHealth: 225
    //maxEnergy: 300
    //speed: 1.0f (medium)
    //primaryFireRate: (irrelevant, charged shot)
    //lockOnRate: 100

    [Header("Bullet")]
    [SerializeField] Bullet _bulletPrefab;
    [SerializeField] float _bulletDamage;
    [SerializeField] float _bulletSpeed;
    [SerializeField] float _bulletDelay;
    [SerializeField] Transform _bulletSpawnPos;
    public float _fireTimer = 5.0f;
    public float _currentCharge = 0.0f;
    public bool _charging = false;
    [SerializeField] float _bulletCost;

    [Header("Missile")]
    [SerializeField] HomingMissile _missilePrefab;
    [SerializeField, Tooltip("This will also affect how many missiles are spawned.")] List<Transform> _missileSpawnPositions;
    [SerializeField] TrackEntitiesInArea _missileRange;
    [SerializeField, Range(0f, 360f)] float _missileLockAngle;
    [SerializeField] float _missileDamage;
    [SerializeField] int _missileLockOnDelay;
    [SerializeField] float _missileCost = 15f;

    bool _missileInputHeld;
    int _lockOnTimer;
    public Entity _missileTarget;

    [Header("EMP")]
    [SerializeField] Bullet _EMPBulletPrefab;
    [SerializeField] float _empCost;
    public float _EmpTimer = 5f;
    public float _EmpDelay = 30f;
    [SerializeField] float _abilityCost = 10f;

    public static float currentProjectileSpeed = 400;

    

    public override void HandleMissile(bool input) {
        // flocking missiles -- or just many missiles with poor tracking
        if (_energy.Value <= 0)
        {
            return;
        }

        if (input) {
            if (_missileRange == null) {
                _missileRange = this.GetComponent<TrackEntitiesInArea>();
            }

            if (_missileTarget != null && _missileRange.Contains(_missileTarget)) {
                if (_lockOnTimer > 0) { _lockOnTimer--; }
            }
            else if (_missileRange.HasAny(out Entity inRange)) {
                _missileTarget = inRange;
                _lockOnTimer = _missileLockOnDelay;
            }

            _missileInputHeld = true;
        }
        // On releasing the key.
        else if (_missileInputHeld) {
            if (_lockOnTimer <= 0 && _missileTarget != null) {
                foreach(var spawnPos in _missileSpawnPositions) {
                    HomingMissile.Create(
                        _missilePrefab,
                        spawnPos.position,
                        _Rbody.velocity*5,
                        _missileTarget,
                        new Attack(_missileDamage, this)
                    );
                }
                LoseEnergy(_missileCost);
            }

            _missileTarget = null;
            _missileInputHeld = false;
            _lockOnTimer = _missileLockOnDelay;
        }
    }

    public override void HandleShoot(bool input) {
        // charged shot

        if (_fireTimer > 0) { _fireTimer--; }

        if (_energy.Value <= 0)
        {
            return;
        }

        if (_charging && !input && _fireTimer <= 0)
        {
            Attack bulletAttack = new Attack(_bulletDamage * (1 + _currentCharge), this);
            bulletAttack.OnHit += ReactToBulletHit;

            Bullet newBullet = Bullet.Create(
                _bulletPrefab,
                bulletAttack,
                _bulletSpawnPos.transform.position,
                this.transform.forward * _bulletSpeed + _Rbody.velocity
            );

            _fireTimer = _bulletDelay;
            LoseEnergy(_bulletCost);
            _currentCharge = 0.0f;
            _charging = false; 
        }
        else if (input)
        {
            _charging = true; 
            _currentCharge += 0.1f;
            _currentCharge = Mathf.Clamp01(_currentCharge);   
            //Debug.Log("Charging" + currentCharge); 
        }
    }

 

    public override void HandleAbility(bool input) {
        // EMP (will require defining a 'stun' method to call on enemies in range)

        if (_EmpTimer > 0) { _EmpTimer-=Time.fixedDeltaTime; }
        if (_energy.Value <= 0)
        {
            return;
        }
        float currentSpeed = 800; 
        if (input && _EmpTimer <= 0)
        {
            Vector3 forward = this.transform.forward;
            if(transform.parent.tag == "Player")
            {
                forward = transform.parent.transform.forward;
                currentSpeed += transform.parent.GetComponent<Rigidbody>().velocity.magnitude; 
            }
            else
            {
                currentSpeed += gameObject.GetComponent<Rigidbody>().velocity.magnitude; 

            }
            LoseEnergy(_empCost);
            Attack bulletAttack = new Attack(0, this);
            bulletAttack.OnHit += ResetAggroEffect.ApplyEffect; // Use this as shorthand to apply an effect on hit.

            Bullet.Create(
                _EMPBulletPrefab,
                bulletAttack,
                _bulletSpawnPos.transform.position,
                forward * currentSpeed
            );

            LoseEnergy(_abilityCost);

            _EmpTimer = _EmpDelay;
        }

    }

    private void ReactToBulletHit(Attack atk, Entity hit)
    {
        hit.AddEffect(new FragileEffect(hit));
        Debug.Log($"Demoman knows that {hit.name} was hit for {atk.Damage} damage.\nApplied a fragile debuff.");
    }

    public override float GetPrimaryFireStatus()
    {
        return _fireTimer; 
    }

    public override float GetSpecialFireStatus()
    {
        return _EmpTimer; 
    }
}