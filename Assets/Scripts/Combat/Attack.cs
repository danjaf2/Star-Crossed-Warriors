using System;
using UnityEngine;

public class Attack {
    public float Damage;
    public Entity Sender;
    public Vector3 Force;

    /// <summary> Full constructor. </summary>
    public Attack(float damage, Entity sender, Vector3 force) {
        Damage = damage;
        Sender = sender;
        Force = force;
        OnHit = null;
    }

    /// <summary> Limited constructor. </summary>
    public Attack(float damage, Entity sender) : this(damage, sender, Vector3.zero) { }
    
    /// <summary> Copy constructor. </summary>
    public Attack(Attack atk) : this(atk.Damage, atk.Sender, atk.Force) { OnHit += atk.OnHit.Invoke; }

    /// <summary> Raised when the attack is passed onto an entity. </summary>
    public event Action<Attack, Entity> OnHit;
    public void Hit(Entity hit) {
        OnHit?.Invoke(this, hit);
    }
}