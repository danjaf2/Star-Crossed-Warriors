using System.Collections;
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

    public override void HandleShoot(bool input) {
        // LASER BEAM!!!
      
        if (_fireTimer > 0) { _fireTimer--; }

        if (input && _fireTimer <= 0)
        {
            Attack bulletAttack = new Attack(_bulletDamage, this);
            bulletAttack.OnHit += ReactToBulletHit;

            Bullet newBullet = Bullet.Create(
                _bulletPrefab,
                bulletAttack,
                spawnPosition.transform.position,
                this.transform.forward * _bulletSpeed
            );

            _fireTimer = _bulletDelay;
        }
    }

    public override void HandleMissile(bool input) {
        // cluster missile / acid cloud missle
    }

    public override void HandleAbility(bool input) {
        //  replusion field / reflective shields
    }

    private void ReactToBulletHit(Attack atk, Entity hit)
    {
        hit.AddEffect(new FragileEffect(hit));
        Debug.Log($"Heavy knows that {hit.name} was hit for {atk.Damage} damage.\nApplied a fragile debuff.");
    }
}