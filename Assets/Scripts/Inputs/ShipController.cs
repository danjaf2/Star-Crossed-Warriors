using System;
using UnityEngine;

public abstract class ShipController : MonoBehaviour {
    // Ship controllers continuously raise events that the ships themselves can subscribe to.
    // This way, a ship that doesn't have missiles doesn't need to subscibe to the missile input event.

    public event Action<Maneuver> GetManeuver;
    public event Action<bool> GetShootInput;
    public event Action<bool> GetMissileInput;
    public event Action<bool> GetAbilityInput;

    protected abstract Maneuver ManeuverInputs();
    protected abstract bool ShootInput();
    protected abstract bool MissileInput();
    protected abstract bool AbilityInput();

    private void Update() {
        GetManeuver.Invoke(ManeuverInputs());
        GetShootInput.Invoke(ShootInput());
        GetMissileInput.Invoke(MissileInput());
        GetAbilityInput.Invoke(AbilityInput());
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