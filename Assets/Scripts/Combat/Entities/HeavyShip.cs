using System.Collections;
using UnityEngine;

public class HeavyShip : PlayerShip {
    //maxHealth: 350
    //maxEnergy: 400
    //speed: 0.5f (slow)
    //primaryFireRate: (continuous)
    //lockOnRate: 100;

    protected override void HandleShoot(bool input) {
        // LASER BEAM!!!
    }

    protected override void HandleMissile(bool input) {
        // cluster missile / acid cloud missle
    }

    protected override void HandleAbility(bool input) {
        //  replusion field / reflective shields
    }
}