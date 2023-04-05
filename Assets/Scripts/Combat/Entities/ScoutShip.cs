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
    float _fireTimer;

    [Header("Missile")]
    [SerializeField] HomingMissile _missilePrefab;

    protected override void HandleAbility(bool input) {
        // boost
    }

    protected override void HandleShoot(bool input) {
        if(_fireTimer > 0) { _fireTimer--; }

        if (input && _fireTimer <= 0) {
            Attack bulletAttack = new Attack(_bulletDamage, this);
            bulletAttack.OnHit += ReactToBulletHit;

            Bullet newBullet = Bullet.Create(
                _bulletPrefab,
                bulletAttack, 
                this.transform.position, 
                this.transform.forward * _bulletSpeed
            );

            _fireTimer = _bulletDelay;
        }
    }

    private void ReactToBulletHit(Attack atk, Entity hit) {
        hit.AddEffect(new FragileEffect(hit));
        Debug.Log($"Scout knows that {hit.name} was hit for {atk.Damage} damage.\nApplied a fragile debuff.");
    }

    protected override void HandleMissile(bool input) {
        // strong homing missile

        // on release
        //HomingMissile.CreateFromPrefab(_missilePrefab, this.transform.position, this.transform.rotation, target);
    }
}