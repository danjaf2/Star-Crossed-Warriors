using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sentry : EnemyShip
{
    [Header("Bullet")]
    [SerializeField] Bullet _bulletPrefab;
    [SerializeField] float _bulletDamage;
    [SerializeField] float _bulletSpeed;
    [SerializeField] float _bulletDelay;
    [SerializeField] GameObject spawnPosition;
    float _fireTimer;

    [Header("Missile")]
    [SerializeField] HomingMissile _missilePrefab;

    public override void HandleAbility(bool input)
    {
        // boost
    }

    public override void HandleShoot(bool input)
    {
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

    private void ReactToBulletHit(Attack atk, Entity hit)
    {
        hit.AddEffect(new FragileEffect(hit));
        Debug.Log($"Sentry knows that {hit.name} was hit for {atk.Damage} damage.\nApplied a fragile debuff.");
    }

    public override void HandleMissile(bool input)
    {
        // strong homing missile

        // on release
        //HomingMissile.CreateFromPrefab(_missilePrefab, this.transform.position, this.transform.rotation, target);
    }
}
