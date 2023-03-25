using System;
using System.Collections.Generic;
using UnityEngine;

// An entity is an object in 3D world-space that can be hit.
public class Entity : MonoBehaviour
{
    protected virtual void Awake() {
        _activeEffects = new List<Effect>();
    }

    public virtual void Hit(Attack atk) {
        ApplyEffectsOnHit(atk);
        atk.Hit(this);
    }

    protected virtual void FixedUpdate() {
        EffectsTick();
    }


    #region Effects
    List<Effect> _activeEffects;

    public void AddEffect(Effect toApply) {
        _activeEffects.Add(toApply);
        _activeEffects.Sort(Effect.PrioritySort);
    }

    public void RemoveEffect(Effect toRemove) {
        if(!_activeEffects.Remove(toRemove)) {
            Debug.LogWarning($"Trying to remove an effect ({toRemove.Name}), that doesn't exist on this entity ({this.name})!", this);
        }
    }

    protected void ApplyEffectsOnHit(Attack toModify) {
        foreach (var item in _activeEffects) {
            item.ModifyHit(toModify);
        }
    }

    protected void EffectsTick() {
        foreach (var effect in _activeEffects) {
            effect.Tick();
        }
    }
    #endregion
}
