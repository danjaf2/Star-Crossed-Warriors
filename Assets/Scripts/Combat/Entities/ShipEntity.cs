using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ShipEntity : EnergizedEntity {

    Rigidbody _rbody;
    protected Vector3 _Direction;

    protected override void Awake() {
        base.Awake();
        _rbody = GetComponent<Rigidbody>();
    }


    public override void Hit(Attack atk) {
        base.Hit(atk);

        // Compute knockback.
        _rbody.AddForce(atk.Force, ForceMode.Impulse);
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