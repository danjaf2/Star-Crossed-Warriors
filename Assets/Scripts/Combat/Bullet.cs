using System;
using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour {

    const int DEFAULT_LIFETIME = 100;

    float _fixedUpateTravelDistance;
    int _lifetime;
    int _layer;

    Vector3 _velocity;
    Attack _toDeliver;


    private void FixedUpdate() {
        if(_lifetime-- <= 0) { 
            Destroy(this.gameObject);
            return;
        }

        // Check if there is anything in the bullet's path (if it is hitting anything).
        if(Physics.Raycast(this.transform.position, _velocity, out var hitInfo, _fixedUpateTravelDistance)) {
            // Check if what's hit is an entity.
            if(hitInfo.collider.TryGetComponent<Entity>(out var entity))
            {
                // Deliver the attack to the hit entity.
                entity.Hit(_toDeliver);
                Destroy(this.gameObject);
            }

            // Destroy the bullet since it hit something.
            if (hitInfo.collider.gameObject.layer == 3)
            {
                 Destroy(this.gameObject);
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

        return newBullet;
    }

    public static Bullet Create(Bullet prefab, Attack attack, Vector3 position, Vector3 velocity, int layer = int.MaxValue) {
        Bullet newBullet = Instantiate(prefab, position, Quaternion.identity);
        newBullet.InitValues(attack, velocity, DEFAULT_LIFETIME, layer);

        return newBullet;
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
}