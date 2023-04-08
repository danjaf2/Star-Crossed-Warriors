using System;
using UnityEngine;

public abstract class ShipController : MonoBehaviour {
    protected PlayerShip _Controlling;

    private void Awake() { Setup(); }

    public void Setup() {
        if (TryGetComponent<PlayerShip>(out var ship)) { _Controlling = ship; }
        else {
            this.enabled = false;
            Debug.Log($"Ship controller expected a {nameof(PlayerShip)} on this GameObject!", this);
        }
    }
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