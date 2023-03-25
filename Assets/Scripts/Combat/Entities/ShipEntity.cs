using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ShipEntity : Entity {

    [Header("Gameplay")]
    [SerializeField] int _maxHealth = 150;
    [SerializeField] int _maxEnergy = 300;

    public float Health { get => _health; }
    float _health;
    public float Energy { get => _energy; }
    float _energy;


    Rigidbody _rbody;
    protected Vector3 _Direction;


    protected override void Awake() {
        base.Awake();
        _health = _maxHealth;
        _energy = _maxEnergy;
        _rbody = GetComponent<Rigidbody>();
    }


    public override void Hit(Attack atk) {
        base.Hit(atk);
        _health -= atk.Damage;

        if(_health <= 0) {
            OnDeath();
            Debug.Log((atk.Sender == null) ? this.name + " was destroyed!" : this.name + $" was destroyed by {atk.Sender.name}!");
            Destroy(this.gameObject);
            return;
        }

        // Compute knockback.
        _rbody.AddForce(atk.Force, ForceMode.Impulse);
    }

    protected void Repair(float hpAmount) {
        _health += hpAmount;
        if (_health > _maxHealth) {
            _health = _maxHealth;
        }
    }
    public void RecoverEnergy(float amount) {
        _energy += amount;
        if(_energy > _maxEnergy) {
            _energy = _maxEnergy;
        }
    }

    protected virtual void OnDeath() {
        Debug.Log(this.name + " was destroyed!");
    }


    // Not sure if any of this will come in useful.

    public static PlayerShip CreateFromPrefab(PlayerShip prefab, Vector3 position) {
        PlayerShip instance = Instantiate(prefab, position, Quaternion.identity);
        instance.Setup();
        return instance;
    }    
    /// <summary>
    /// Perform whatever needed step is required on instantiation.
    /// </summary>
    protected virtual void Setup() {

    }
}