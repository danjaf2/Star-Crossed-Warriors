using UnityEngine;

public abstract class ManeuverableController : MonoBehaviour {

    Maneuverable _maneuvering;

    // Might want to rework this. Other way to do this would have all
    // entities above a certain hierarchical level be considered as maneuverable.
    // (i.e. define a ManeuverableEntity class which controllers will target)
    private void Awake() {
        // Unity's GetComponent() cannot find interfaces,
        // so we need to perform the search through C# reflection.
        Component[] components = this.gameObject.GetComponents<Component>();
        foreach (var compo in components) {
            foreach (var inter in compo.GetType().GetInterfaces()) {
                if(inter == typeof(Maneuverable)) {
                    _maneuvering = (Maneuverable)compo;
                }
            }
        }
    }

    public abstract Maneuver GetManeuver();

    private void Update() {
        _maneuvering.Maneuver(GetManeuver());
    }
}