using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class Attack {
    public float Damage;
    public Entity Sender;
    public Vector3 Force;

    public Attack(float damage, Entity sender) {
        Damage = damage;
        Sender = sender;
    }
    public Attack(float damage, Entity sender, Vector3 force) {
        Damage = damage;
        Sender = sender;
        Force = force;
    }

    public event Action<Attack, Entity> OnHit;
    public void Hit(Entity hit) {
        OnHit?.Invoke(this, hit);
    }
}