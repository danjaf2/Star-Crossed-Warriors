using System.Collections;
using UnityEngine;

public class DemoShip : PlayerShip {

    //maxHealth: 225
    //maxEnergy: 300
    //speed: 1.0f (medium)
    //primaryFireRate: (irrelevant, charged shot)
    //lockOnRate: 100

    public override void HandleMissile(bool input) {
        // flocking missiles -- or just many missiles with poor tracking
    }

    public override void HandleShoot(bool input) {
        // charged shot
    }

    public override void HandleAbility(bool input) {
        // EMP (will require defining a 'stun' method to call on enemies in range)
    }
}