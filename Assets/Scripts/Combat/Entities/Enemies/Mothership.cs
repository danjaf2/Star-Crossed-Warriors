using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mothership : EnemyShip
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
        //Shields
        //throw new System.NotImplementedException();
    }

    public override void HandleMissile(bool input)
    {
        //throw new System.NotImplementedException();
    }

    public override void HandleShoot(bool input)
    { //EMP
       // throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
