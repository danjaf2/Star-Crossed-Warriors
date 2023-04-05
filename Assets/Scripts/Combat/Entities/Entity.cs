using System;
using System.Collections.Generic;
using UnityEngine;

// An entity is an object in 3D world-space that can be hit.
public class Entity : MonoBehaviour {

    [Header("Gameplay")]
    [SerializeField] int _maxHealth = 150;

    public float Health { get => _health; }
    float _health;

    public event Action<Attack> OnHit;
    public event Action OnTick;


    public virtual void Hit(Attack atk) {
        ApplyEffectsOnHit(atk);

        _health -= atk.Damage;
        if (_health <= 0) {
            if (atk.Sender == null) { Debug.Log(this.name + " was destroyed!"); }
            else { Debug.Log(this.name + $" was destroyed by {atk.Sender.name}!"); }

            OnDeath();
        }

        atk.Hit(this);
        OnHit?.Invoke(atk);
    }

    protected void Repair(float hpAmount) {
        _health += hpAmount;
        if (_health > _maxHealth) {
            _health = _maxHealth;
        }
    }
    protected virtual void OnDeath() {
        Destroy(this.gameObject);
    }


    #region Lifetime
    protected virtual void Awake() {
        _health = _maxHealth;
        _activeEffects = new List<Effect>();
    }

    protected virtual void FixedUpdate() {
        OnTick?.Invoke();
    }
    #endregion


    #region Effects
    List<Effect> _activeEffects;

    public void AddEffect(Effect toApply) {
        // Ideally, effects that stack would instead have a counter to
        // know how many there are instead of having actual duplicates.
        if(toApply.Stacks && HasEffect(toApply, out var present)) { present.ResetDuration(); }
        else { _activeEffects.Add(toApply); }

        _activeEffects.Sort(Effect.PrioritySort);
    }

    bool HasEffect(Effect check, out Effect present) {
        foreach (var effect in _activeEffects) {
            if(check.GetType() == effect.GetType()) {
                present = effect;
                return true;
            }
        }
        present = null;
        return false;
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
    #endregion
}
