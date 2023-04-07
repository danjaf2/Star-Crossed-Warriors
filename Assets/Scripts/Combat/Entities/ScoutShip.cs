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
    [SerializeField] GameObject bulletSpawnPosition;
    float _fireTimer;

    [Header("Missile")]
    [SerializeField] HomingMissile _missilePrefab;
    [SerializeField] float _missileDamage;

    [SerializeField] KeepTrackWithinArea<Entity> _missileRange;
    [SerializeField] int _missileLockOnDelay;

    bool _missileInputHeld;
    int _lockOnTimer;
    Entity _missileTarget;

    [Header("SpecialAbility")]
    [SerializeField] float _boostDelay;
    [SerializeField] float _boostCoolDown;
    [SerializeField] float _boostMultiplier = 5.0f;

    public override void HandleAbility(bool input) {
        // boost
        if (_boostCoolDown > 0) { _boostCoolDown--; }

        if (input && _boostCoolDown <= 0)
        {
            LoseEnergy(_bulletCost);
           
            _boostCoolDown = _boostDelay;

  
            if(transform.parent.tag == "Player")
            {
                Debug.Log("Player boosted");
                transform.parent.GetComponent<PlaneController>().BoostForward(_boostMultiplier);
            }
            else
            {
                gameObject.GetComponent<Vehicle>().BoostForward(_boostMultiplier); 
            }

          
            Debug.Log("Scout boosted"); 

        }
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
                bulletSpawnPosition.transform.position, 
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