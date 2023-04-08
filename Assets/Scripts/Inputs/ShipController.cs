using System;
using UnityEngine;

public abstract class ShipController : MonoBehaviour {

    PlayerShip _controlling;

    protected bool _ShootInput;
    protected bool _MissileInput;
    protected bool _AbilityInput;


    private void Awake() {
        Setup();
    }

    public void Setup() {
        if (TryGetComponent<PlayerShip>(out var ship)) { _controlling = ship; }
        else {
            this.enabled = false;
            Debug.Log($"Ship controller expected a {nameof(PlayerShip)} on this GameObject!", this);
        }
    }

    protected virtual void FixedUpdate() {
        _controlling.SetAbilityInput(_AbilityInput);
        _controlling.SetMissileInput(_MissileInput);
        _controlling.SetShootInput(_ShootInput);
    }



    // This isn't used since the flying part of ships is on another component.
    protected abstract Maneuver GetManeuverInputs();
}

// Old implementation

//// Might want to rework this. Other way to do this would have all
//// entities above a certain hierarchical level be considered as maneuverable.
//// (i.e. define a ManeuverableEntity class which controllers will target)
//private void Awake() {
//    // Unity's GetComponent() cannot find interfaces,
//    // so we need to perform the search through C# reflection.
//    Component[] components = this.gameObject.GetComponents<Component>();
//    foreach (var compo in components) {
//        foreach (var inter in compo.GetType().GetInterfaces()) {
//            if(inter == typeof(Maneuverable)) {
//                _maneuvering = (Maneuverable)compo;
//            }
//        }
//    }
//}