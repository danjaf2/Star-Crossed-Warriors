using System.Collections;
using UnityEngine;

public class KeyboardController : ShipController {
    protected override Maneuver ManeuverInputs() {
        return new Maneuver(
                Input.GetKey(KeyCode.W).ToByte() - Input.GetKey(KeyCode.S).ToByte(),
                Input.GetKey(KeyCode.D).ToByte() - Input.GetKey(KeyCode.A).ToByte(),
                Input.GetKey(KeyCode.E).ToByte() - Input.GetKey(KeyCode.Q).ToByte(),
                Input.GetKey(KeyCode.LeftShift).ToByte() - Input.GetKey(KeyCode.LeftControl).ToByte()
            );
    }

    protected override bool ShootInput() {
        return Input.GetKey(KeyCode.Mouse0);
    }
    protected override bool MissileInput() {
        return Input.GetKey(KeyCode.Mouse1);
    }
    protected override bool AbilityInput() {
        return Input.GetKey(KeyCode.Space);
    }
}