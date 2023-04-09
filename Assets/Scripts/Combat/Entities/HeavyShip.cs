using System.Collections;
using UnityEditor.TextCore.Text;
using UnityEngine;

public class HeavyShip : PlayerShip {
    //maxHealth: 350
    //maxEnergy: 400
    //speed: 0.5f (slow)
    //primaryFireRate: (continuous)
    //lockOnRate: 100;

    [Header("Bullet")]
    [SerializeField] Bullet _bulletPrefab;
    [SerializeField] float _bulletDamage;
    [SerializeField] float _bulletSpeed;
    [SerializeField] float _bulletDelay;
    [SerializeField] GameObject spawnPosition;
    float _fireTimer = 5.0f;

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
    [SerializeField] bool shieldIsActive; 
    [SerializeField] float _shieldHPBase = 400;
    [SerializeField] float _shieldCurrentHP;
    [SerializeField] float _shieldDelay;
    [SerializeField] float _shieldResetTimer;
    [SerializeField] GameObject shieldWeak;
    [SerializeField] GameObject shieldStrong;


    public override void HandleShoot(bool input) {
        // LASER BEAM!!!
      
        if (_fireTimer > 0) { _fireTimer--; }
        float currentSpeed = 800;
        Vector3 forward = this.transform.forward;
        if (transform.parent.tag == "Player")
        {
            forward = transform.parent.transform.forward;
            currentSpeed += transform.parent.GetComponent<Rigidbody>().velocity.magnitude;
        }
        else
        {
            currentSpeed += gameObject.GetComponent<Rigidbody>().velocity.magnitude;

        }
        if (input && _fireTimer <= 0)
        {
            Attack bulletAttack = new Attack(_bulletDamage, this);
            bulletAttack.OnHit += ReactToBulletHit;

            Bullet newBullet = Bullet.Create(
                _bulletPrefab,
                bulletAttack,
                spawnPosition.transform.position,
                this.transform.forward *( _bulletSpeed+currentSpeed)
            );

            _fireTimer = _bulletDelay;
        }
    }

    public override void HandleMissile(bool input) {
        // cluster missile / acid cloud missle

        if (input)
        {
            if (_missileRange == null)
            {
                _missileRange = this.GetComponent<TrackEntitiesInArea>();
            }

            if (_missileTarget != null && _missileRange.Contains(_missileTarget))
            {
                if (_lockOnTimer > 0) { _lockOnTimer--; }
            }
            else if (_missileRange.HasAny(out Entity inRange))
            {
                Debug.Log(inRange);
                _missileTarget = inRange;
                _lockOnTimer = _missileLockOnDelay;
            }

            _missileInputHeld = true;
        }
        // On releasing the key.
        else if (_missileInputHeld)
        {
            if (_lockOnTimer <= 0 && _missileTarget != null)
            {
                float speed = 0;

                if (transform.parent.TryGetComponent(out Rigidbody rb))
                {
                    speed = rb.velocity.magnitude + _missileBaseSpeed;
                }
                else
                {
                    if (transform.TryGetComponent(out Rigidbody rb2))
                    {
                        speed = rb2.velocity.magnitude + _missileBaseSpeed;
                    }
                }

                HomingMissile.Create(
                _missilePrefab,
                missileSpawnPosition.transform.position,
                this.transform.rotation,
                _missileTarget,
                new Attack(_missileDamage, this),
                speed
            );

            }

            _missileTarget = null;
            _missileInputHeld = false;
            _lockOnTimer = _missileLockOnDelay;
        }
    }

    public override void HandleAbility(bool input) {
        //  replusion field / reflective shields
        

        if (_shieldResetTimer > 0) {_shieldResetTimer--; }

        if (input && _shieldResetTimer <= 0 && !shieldIsActive)
        {
            LoseEnergy(100);
            shieldIsActive = true;
            _shieldCurrentHP = _shieldHPBase; 


            _shieldResetTimer = _shieldDelay ;

            shieldStrong.SetActive(true);
            shieldWeak.SetActive(false);

            Debug.Log("Shield Is Active"); 
        }
        else if (_shieldResetTimer <= 0 && shieldIsActive)
        {
            shieldIsActive = false;
            shieldStrong.SetActive(false);
            shieldWeak.SetActive(false);
            _shieldResetTimer = _shieldDelay;
        }
        



    }

    public override void Hit(Attack atk)
    {
        ApplyEffectsOnHit(atk);

        if(shieldIsActive)
        {
            _shieldCurrentHP -= atk.Damage;
            if (_shieldCurrentHP <= 0)
            {
                shieldIsActive = false;

                shieldStrong.SetActive(false);
                shieldWeak.SetActive(false);
            }
            else if (_shieldCurrentHP <= _shieldHPBase/4) {
                shieldIsActive = true;
                shieldStrong.SetActive(false);
                shieldWeak.SetActive(true);
            }

        }
        else
        {
            _health -= atk.Damage;
        }
        
        if (_health <= 0)
        {
            if (atk.Sender == null) { Debug.Log(this.name + " was destroyed!"); }
            else { Debug.Log(this.name + $" was destroyed by {atk.Sender.name}!"); }

            OnDeath();
        }
        else
        {
            if (atk.Sender == null) { Debug.Log(this.name + " was shoot!"); }
            else { Debug.Log(this.name + $" was shoot by {atk.Sender.name}!"); }
        }

        atk.Hit(this);
        InvokeAttack(atk);
    }


    private void ReactToBulletHit(Attack atk, Entity hit)
    {
        hit.AddEffect(new FragileEffect(hit));
        Debug.Log($"Heavy knows that {hit.name} was hit for {atk.Damage} damage.\nApplied a fragile debuff.");
    }
}