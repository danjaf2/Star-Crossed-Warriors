 using System.Collections;
using UnityEngine;

public class KeyboardController : ShipController {
    private void Update() {
        _AbilityInput = Input.GetKey(KeyCode.Tab);
        _MissileInput = Input.GetKey(KeyCode.LeftShift);
        _ShootInput = Input.GetKey(KeyCode.Space);
    }

    protected override Maneuver GetManeuverInputs() {
        return new Maneuver(
                Input.GetKey(KeyCode.W).ToByte() - Input.GetKey(KeyCode.S).ToByte(),
                Input.GetKey(KeyCode.D).ToByte() - Input.GetKey(KeyCode.A).ToByte(),
                Input.GetKey(KeyCode.E).ToByte() - Input.GetKey(KeyCode.Q).ToByte(),
                Input.GetKey(KeyCode.LeftShift).ToByte() - Input.GetKey(KeyCode.LeftControl).ToByte()
            );
    }
}