using System;
using System.Collections.Generic;
using UnityEngine;

// An entity is something in world-space that can be hit.
public class Entity : MonoBehaviour
{
    public virtual void Hit(Attack atk) { }

    protected virtual void FixedUpdate() { }

#region Effects (Buffs/Debuffs)
    List<Effect> _activeEffects;

    /// <summary>
    /// Applies 
    /// </summary>
    protected Attack ApplyEffectsOnHit(Attack toModify) {
        foreach (var item in _activeEffects) {
            toModify = item.OnHit(toModify);
        }
        return toModify;
    }
#endregion
}
