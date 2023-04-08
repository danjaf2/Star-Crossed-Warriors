using System;
using UnityEngine;

public class Attack {
    public float Damage;
    public Entity Sender;
    public Vector3 Force;

    /// <summary> Full constructor. </summary>
    public Attack(float damage, Entity sender, Vector3 force, Action<Attack, Entity> onHit) {
        Damage = damage;
        Sender = sender;
        Force = force;
        OnHit = onHit;
    }

    /// <summary> Limited constructor. </summary>
    public Attack(float damage, Entity sender) : this(damage, sender, Vector3.zero, null) { }
    /// <summary> Less limited constructor. </summary>
    public Attack(float damage, Entity sender, Vector3 force) : this(damage, sender, force, null) { }

    /// <summary> Copy constructor. </summary>
    public Attack(Attack atk) : this(atk.Damage, atk.Sender, atk.Force, atk.OnHit) { }

    /// <summary> Raised when the attack is passed onto an entity. </summary>
    public event Action<Attack, Entity> OnHit;
    public void Hit(Entity hit) {
        OnHit?.Invoke(this, hit);
    }
}