using System.Collections;
using UnityEngine;

public class ScoutShip : PlayerShip {

    //maxHealth: 150
    //maxEnergy: 300
    //speed: 2 (fast)
    //primaryFireRate: (medium)
    //lockOnRate: 100

    [Header("Bullet")]
    [SerializeField] Bullet _bulletPrefab;
    [SerializeField] float _bulletDamage;
    [SerializeField] float _bulletSpeed;
    [SerializeField] float _bulletDelay;
    [SerializeField] float _bulletCost;
    [SerializeField] GameObject spawnPosition;
    float _fireTimer;

    [Header("Missile")]
    [SerializeField] HomingMissile _missilePrefab;
    [SerializeField] float _missileDamage;

    [SerializeField] KeepTrackWithinArea<Entity> _missileRange;
    [SerializeField] int _missileLockOnDelay;

    bool _missileInputHeld;
    int _lockOnTimer;
    Entity _missileTarget;

    public override void HandleAbility(bool input) {
        // boost
    }

    public override void HandleShoot(bool input) {
        if(_fireTimer > 0) { _fireTimer--; }

        if (input && _fireTimer <= 0) {
            LoseEnergy(_bulletCost);
            Attack bulletAttack = new Attack(_bulletDamage, this);
            bulletAttack.OnHit += ReactToBulletHit;

            Bullet.Create(
                _bulletPrefab,
                bulletAttack,
                spawnPosition.transform.position, 
                this.transform.forward * _bulletSpeed
            );

            _fireTimer = _bulletDelay;
        }
    }

    private void ReactToBulletHit(Attack atk, Entity hit) {
        hit.AddEffect(new FragileEffect(hit));
        Debug.Log($"Scout knows that {hit.name} was hit for {atk.Damage} damage.\nApplied a fragile debuff.");
    }

    public override void HandleMissile(bool input) {
        // strong homing missile

        // on release
        //HomingMissile.CreateFromPrefab(_missilePrefab, this.transform.position, this.transform.rotation, target);
        // When holding the key down.
        if (input) {
            if (_missileTarget != null && _missileRange.Contains(_missileTarget)) {
                if (_lockOnTimer > 0) { _lockOnTimer--; }
            }
            else if (_missileRange.HasAny(out var inRange)) {
                _missileTarget = inRange;
                _lockOnTimer = _missileLockOnDelay;
            }

            _missileInputHeld = true;
        }
        // On releasing the key.
        else if (_missileInputHeld) {
            if (_lockOnTimer <= 0) {
                HomingMissile.Create(
                    _missilePrefab,
                    this.transform.position,
                    this.transform.rotation,
                    _missileTarget,
                    new Attack(_missileDamage, this)
                );
            }

            _missileTarget = null;
            _missileInputHeld = false;
            _lockOnTimer = _missileLockOnDelay;
        }
    }
}