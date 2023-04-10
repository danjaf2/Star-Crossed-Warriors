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
    [SerializeField] PayloadMissile _missilePrefab;
    [SerializeField] Transform _missileSpawnPos;
    [SerializeField] float _missileCost;
    [SerializeField] float _missileDelay;
    bool _missileInputHeld;
    public float _missileTimer = 5.0f;

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
                this.transform.forward *( _bulletSpeed)
            );

            _fireTimer = _bulletDelay;
        }
    }

    public override void HandleMissile(bool input) {
        // cluster missile / acid cloud missle
        if (_missileTimer > 0) { _missileTimer-= Time.fixedDeltaTime; }
        if (input) {
            _missileInputHeld = true;
        }
        else if(_missileInputHeld&& _missileTimer <= 0) {
            _missileTimer = _missileDelay;
            _missileInputHeld = false;
            PayloadMissile.Create(_missilePrefab, _missileSpawnPos.position, _Rbody.velocity*5);
            LoseEnergy(_missileCost);
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
            _health.Value -= atk.Damage;
        }
        
        if (_health.Value <= 0)
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

    public override float GetPrimaryFireStatus()
    {
        return _fireTimer;
    }

    public override float GetSpecialFireStatus()
    {
        return _shieldResetTimer;
    }
}