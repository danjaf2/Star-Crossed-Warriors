﻿using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Unity.Netcode;

public enum BulletVariant { REGULAR, EMP, MINE}; 
public class Bullet :  NetworkBehaviour{

    const int DEFAULT_LIFETIME = 100;

    float _fixedUpateTravelDistance;
    int _lifetime;
    int _layer;

    Vector3 _velocity;
    Attack _toDeliver;

    public BulletVariant _variant = BulletVariant.REGULAR;
    public float _EMPRange = 100f;
    public float _explosionRange = 100f;


    private void FixedUpdate() {
        if(_lifetime-- <= 0 && _variant != BulletVariant.MINE) {
            //Destroy(this.gameObject);
            PerformActionOnCollision(_variant); 
            return;
        }

        // Check if there is anything in the bullet's path (if it is hitting anything).
        if(Physics.Raycast(this.transform.position, _velocity, out var hitInfo, _fixedUpateTravelDistance)) {
            // Check if what's hit is an entity.
            if(hitInfo.collider.TryGetComponent<Entity>(out var entity))
            {
                // Deliver the attack to the hit entity.
                entity.Hit(_toDeliver);

                PerformActionOnCollision(_variant);
                //Destroy(this.gameObject);
            }

            // Destroy the bullet since it hit something.
            if (hitInfo.collider.gameObject.layer == 3)
            {
                PerformActionOnCollision(_variant);
                //Destroy(this.gameObject);
            }
           
        }
        // Otherwise, move forwards.
        else { this.transform.position += _velocity; }
    }

    /// <summary>
    /// Equivalent of a constructor for a bullet instance.
    /// </summary>
    public static Bullet Create(Attack attack, Vector3 position, Vector3 velocity, int layer = int.MaxValue) {
        GameObject gameObj = new GameObject(nameof(Bullet));
        gameObj.transform.position = position;

        Bullet newBullet = gameObj.AddComponent<Bullet>();
        newBullet.InitValues(attack, velocity, DEFAULT_LIFETIME, layer);
        GameObject.FindObjectOfType<AudioManager>().playAudio(AudioCategory.SHOOT, 0);
        return newBullet;
    }

    public static Bullet Create(Bullet prefab, Attack attack, Vector3 position, Vector3 velocity, int layer = int.MaxValue) {
        Bullet newBullet = Instantiate(prefab, position, Quaternion.identity);
        newBullet.InitValues(attack, velocity, DEFAULT_LIFETIME, layer);
        newBullet.TestServerRpc();

        GameObject.FindObjectOfType<AudioManager>().playAudio(AudioCategory.SHOOT, 0); 
        return newBullet;
    }
    [ServerRpc(RequireOwnership =false)]
    private void TestServerRpc()
    {
        Debug.Log("THIS IS WHAT SERVER SEES");
        this.transform.GetComponent<NetworkObject>().Spawn(true);
        TestClientRpc();
    }
    [ClientRpc]
    private void TestClientRpc()
    {
        Debug.Log("THIS IS WHAT CLIENT SEES");
        this.transform.GetComponent<NetworkObject>().Spawn(true);
    }

    void InitValues(Attack attack, Vector3 velocity, int lifetime, int layer) {
        _toDeliver = attack;
        _velocity = velocity;
        _lifetime = lifetime;
        _layer = layer;

        // Do in advance calculations that would need to be done on each frame.
        _velocity *= Time.fixedDeltaTime;
        _fixedUpateTravelDistance = _velocity.magnitude;
    }

    public void PerformActionOnCollision(BulletVariant type)
    {
        switch(type)
        {
            case BulletVariant.REGULAR:
                Destroy(this.gameObject);
                break;
            case BulletVariant.EMP:
                PerformEMPBlast(); 
                Destroy(this.gameObject, 3f);
                break;
            case BulletVariant.MINE:
                Explode(); 
                Destroy(this.gameObject, 3f);
                break;
            default:
                Destroy(this.gameObject);
                break; //Do nothing
        }
    }

    public void PerformEMPBlast()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _EMPRange);
        foreach (var hitCollider in hitColliders)
        {
            PlayerShip ship = hitCollider.gameObject.GetComponent<PlayerShip>();
            if (ship != null)
            {
                ship.BeStunned();
                //Debug.Log("Ship was stunned"); 
            }
        }

        //Instantiate EMP effect

        GameObject.FindObjectOfType<ParticleManager>().InstantiateEMP(gameObject); 

    }

    public void Explode()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _explosionRange);
        foreach (var hitCollider in hitColliders)
        {
            PlayerShip ship = hitCollider.gameObject.GetComponent<PlayerShip>();

            if(hitCollider.gameObject.tag == "Player")
            {
                ship = hitCollider.gameObject.GetComponentInChildren<PlayerShip>();
            }
            if (ship != null && _toDeliver != null)
            {
                _toDeliver = new Attack(100, gameObject.GetComponent<MineField>());
                ship.Hit(_toDeliver);
               // Debug.Log("MINE - " + ship.name + " was hit by an explosion");
            }
            else if (ship != null)
            {
                _toDeliver = new Attack(100, gameObject.GetComponent<MineField>());
                ship.Hit(_toDeliver); 
                //Debug.Log("MINE - " + ship.name + " was hit by an explosion");
            }
        }

        GameObject.FindObjectOfType<ParticleManager>().InstantiateExplosion2(gameObject);

    }

}