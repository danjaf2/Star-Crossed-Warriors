using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

// An entity is an object in 3D world-space that can be hit.
public class Entity : NetworkBehaviour {

    [Header("Gameplay")]
    [SerializeField] public int _maxHealth = 150;

    public float Health { get => _health.Value; }
    public NetworkVariable<float> _health = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public event Action<Attack> OnHit;
    public event Action OnTick;


    public virtual void Hit(Attack atk) {
        ApplyEffectsOnHit(atk);

        if(IsOwner) {
        _health.Value -= atk.Damage;
        }
        else
        {
            DamageServerRpc(atk.Damage);
        }
        if (_health.Value <= 0) {
            if (atk.Sender == null) { Debug.Log(this.name + " was destroyed!"); }
            else { Debug.Log(this.name + $" was destroyed by {atk.Sender.name}!"); }
            if (IsOwner)
            {
                _health.Value = -10;
            }
            else {
                //DamageServerRpc(-1000);
            }
            //OnDeath();
        }
        else
        {
            if (atk.Sender == null) { Debug.Log(this.name + " was shoot!"); }
            else { Debug.Log(this.name + $" was shoot by {atk.Sender.name}!"); }
        }

        atk.Hit(this);
        OnHit?.Invoke(atk);
    }

    [ServerRpc(RequireOwnership =false)]
    private void DamageServerRpc(float damage)
    {
        _health.Value -= damage;
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetServerRpc(float health)
    {
        _health.Value = health;
    }

    public void InvokeAttack(Attack atk)
    {
        OnHit?.Invoke(atk);
    }

    protected void Repair(float hpAmount) {
        if (IsOwner)
        {
            _health.Value += hpAmount;
        }
        if (_health.Value > _maxHealth) {
            if (IsOwner)
            {
                _health.Value = _maxHealth;
            }
        }
    }
    protected virtual void OnDeath() {
        if(TryGetComponent<SimpleObjective>(out SimpleObjective obj))
        {
            ObjectiveManager.Instance.OnObjectiveComplete(obj);
        }

        if(TryGetComponent(out PlayerShip ship))
        {
                if(ship.transform.parent.TryGetComponent(out Rigidbody body))
                {
                    body.velocity = Vector3.zero;
                    body.transform.position = new Vector3(0,0,0);

                Debug.Log(_health.Value);
                }
                else
                {
                    if (ship.transform.TryGetComponent(out Rigidbody body2))
                    {
                    if (ship.respawnPosition != null)
                    {
                        body2.velocity = Vector3.zero;
                        body2.transform.position = ship.respawnPosition.transform.position;
                    }
                    }
                }
            ship.GetComponent<Entity>().Repair(1000);
        }
        else
        {
            ParticleManager.Instance.InstantiateExplosion(this.gameObject);
            Destroy(this.gameObject, 0.01f);
        }

        
    }


    #region Lifetime
    protected virtual void Awake() {
        _health.Value = _maxHealth;
        _activeEffects = new List<Effect>();
    }

    protected virtual void FixedUpdate() {
        
        if (_health.Value < 0)
        {
            OnDeath();
        }

        OnTick?.Invoke();
    }
    #endregion


    #region Effects
    List<Effect> _activeEffects;

    public void AddEffect(Effect toApply) {
        // Ideally, effects that stack would instead have a counter to
        // know how many there are instead of having actual duplicates.
        if(!toApply.Stacks && HasEffect(toApply, out var present)) { present.ResetDuration(); }
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
        _activeEffects.Remove(toRemove);
    }

    protected void ApplyEffectsOnHit(Attack toModify) {
        foreach (var item in _activeEffects) {
            item.ModifyHit(toModify);
        }
    }
    #endregion
}
