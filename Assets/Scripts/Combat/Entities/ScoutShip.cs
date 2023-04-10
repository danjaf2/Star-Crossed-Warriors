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
    [SerializeField] GameObject missileSpawnPosition;
    [SerializeField] float _missileBaseSpeed;
    [SerializeField] TrackEntitiesInArea _missileRange;
    [SerializeField] int _missileLockOnDelay;

    bool _missileInputHeld;
    int _lockOnTimer;
    public Entity _missileTarget;

    [Header("SpecialAbility")]
    [SerializeField] float _boostDelay;
    [SerializeField] float _boostCoolDown;
    [SerializeField] float _boostMultiplier = 5.0f;

   

    private void Start()
    {
       
    }
    public override void HandleAbility(bool input) {
        // boost
        if (_boostCoolDown > 0) { 
            _boostCoolDown-= Time.deltaTime;
            return;
        }

        if (input && _boostCoolDown <= 0)
        {
            LoseEnergy(_bulletCost);
           
            _boostCoolDown = _boostDelay;

  
            if(transform.parent.tag == "Player")
            {
                //Debug.Log("Player boosted");
                transform.parent.GetComponent<PlaneController>().BoostForward(_boostMultiplier);
            }
            else
            {
                gameObject.GetComponent<Vehicle>().BoostForward(_boostMultiplier); 
            }

          
            //Debug.Log(this.gameObject.name); 

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
                this.transform.forward * _bulletSpeed + _Rbody.velocity
            );

            _fireTimer = _bulletDelay;
        }
    }

    private void ReactToBulletHit(Attack atk, Entity hit) {
        hit.AddEffect(new FragileEffect(hit));
        hit.AddEffect(new ResetAggroEffect(hit));
        Debug.Log($"Scout knows that {hit.name} was hit for {atk.Damage} damage.\nApplied a fragile debuff.");
    }

    // strong homing missile
    public override void HandleMissile(bool input) {
        // When holding the key down.
        

        if (input) {
            if (_missileRange == null)
            {
                _missileRange = this.GetComponent<TrackEntitiesInArea>();
            }

            if (_missileTarget != null && _missileRange.Contains(_missileTarget)) {
                if (_lockOnTimer > 0) { _lockOnTimer--; }
            }
            else if (_missileRange.HasAny(out Entity inRange)) {
                Debug.Log(inRange);
                _missileTarget = inRange;
                _lockOnTimer = _missileLockOnDelay;
            }

            _missileInputHeld = true;
        }
        // On releasing the key.
        else if (_missileInputHeld) {
            if (_lockOnTimer <= 0 && _missileTarget != null) {

                HomingMissile.Create(
                    _missilePrefab,
                    missileSpawnPosition.transform.position,
                    _Rbody.velocity,
                    _missileTarget,
                    new Attack(_missileDamage, this)
                );
            }

            _missileTarget = null;
            _missileInputHeld = false;
            _lockOnTimer = _missileLockOnDelay;
        }
    }

    public override float GetPrimaryFireStatus()
    {
        return _fireTimer;
    }

    public override float GetSpecialFireStatus()
    {
        return _boostCoolDown;
    }
}