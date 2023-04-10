using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ShipEntity : EnergizedEntity {

    protected Rigidbody _Rbody;
    protected Vector3 _Direction;

    protected override void Awake() {
        base.Awake();
        try { 
        if (transform.parent.TryGetComponent(out Rigidbody rb))
        {
            _Rbody = rb;
        }
        else
        {
            if (transform.TryGetComponent(out Rigidbody rb2))
            {
                _Rbody = rb2;
            }
        }
        }catch(System.Exception ex)
        {
            Debug.Log(this.gameObject.name);
        }

    }



    public override void Hit(Attack atk) {
        base.Hit(atk);

        // Compute knockback.
        _Rbody.AddForce(atk.Force, ForceMode.Impulse);
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