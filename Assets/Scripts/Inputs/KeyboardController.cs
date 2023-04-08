 using System.Collections;
using UnityEngine;

public class KeyboardController : ShipController {

    private void Update() {
        _Controlling.SetShootInput(Input.GetKey(KeyCode.Space));
        _Controlling.SetAbilityInput(Input.GetKey(KeyCode.Tab));
        _Controlling.SetMissileInput(Input.GetKey(KeyCode.LeftShift));
    }

    // Unused Maneuver Get
    //protected override Maneuver GetManeuverInputs() {
    //    return new Maneuver(
    //            Input.GetKey(KeyCode.W).ToByte() - Input.GetKey(KeyCode.S).ToByte(),
    //            Input.GetKey(KeyCode.D).ToByte() - Input.GetKey(KeyCode.A).ToByte(),
    //            Input.GetKey(KeyCode.E).ToByte() - Input.GetKey(KeyCode.Q).ToByte(),
    //            Input.GetKey(KeyCode.LeftShift).ToByte() - Input.GetKey(KeyCode.LeftControl).ToByte()
    //        );
    //}
}