﻿using System;
using System.Collections;
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
    [SerializeField] GameObject spawnPosition;
    public float _fireTimer = 5.0f;
    public float currentCharge = 0.0f;
    public bool charging = false;

    [Header("Missile")]
    [SerializeField] HomingMissile _missilePrefab;

    [Header("EMP")]
    [SerializeField] Bullet _EMPBulletPrefab;
    [SerializeField] float _empCost;
    public float _EmpTimer = 5f;
    public float _EmpDelay = 30f;

    public static float currentProjectileSpeed = 400;

    public bool playerControlled = false;

    

   

    public override void HandleMissile(bool input) {
        // flocking missiles -- or just many missiles with poor tracking
    }

    public override void HandleShoot(bool input) {
        // charged shot
        if(playerControlled) {
        if (transform.parent.gameObject.GetComponent<NetworkBehaviour>()!=null)
        {
            if (!transform.parent.gameObject.GetComponent<NetworkBehaviour>().IsOwner)
            {
                return;
            }
        }
        }

        if (_fireTimer > 0) { _fireTimer--; }


        if (charging && !input && _fireTimer <= 0)
        {
            Attack bulletAttack = new Attack(_bulletDamage * (1 + currentCharge), this);
            bulletAttack.OnHit += ReactToBulletHit;

            Bullet newBullet = Bullet.Create(
                _bulletPrefab,
                bulletAttack,
                spawnPosition.transform.position,
                this.transform.forward * _bulletSpeed
            );

            _fireTimer = _bulletDelay;

            currentCharge = 0.0f;
            charging = false; 
        }
        else if (input)
        {
            charging = true; 
            currentCharge += 0.1f;
            currentCharge = Mathf.Clamp01(currentCharge);   
            //Debug.Log("Charging" + currentCharge); 
        }
       
      


    }

 

    public override void HandleAbility(bool input) {
        // EMP (will require defining a 'stun' method to call on enemies in range)

        if (playerControlled)
        {
            if (transform.parent.gameObject.GetComponent<NetworkBehaviour>() != null)
            {
                if (!transform.parent.gameObject.GetComponent<NetworkBehaviour>().IsOwner)
                {
                    return;
                }
            }
        }

        if (_EmpTimer > 0) { _EmpTimer-=Time.fixedDeltaTime; }

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
            bulletAttack.OnHit += ReactToBulletHit;

            Bullet.Create(
                _EMPBulletPrefab,
                bulletAttack,
                spawnPosition.transform.position,
                forward * currentSpeed
            );

            _EmpTimer = _EmpDelay;
        }

    }

    private void ReactToBulletHit(Attack atk, Entity hit)
    {
        hit.AddEffect(new FragileEffect(hit));
        hit.AddEffect(new ResetAggroEffect(hit));
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