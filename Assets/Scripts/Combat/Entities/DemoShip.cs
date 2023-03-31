using System.Collections;
using UnityEngine;

public class DemoShip : PlayerShip {

    //maxHealth: 225
    //maxEnergy: 300
    //speed: 1.0f (medium)
    //primaryFireRate: (irrelevant, charged shot)
    //lockOnRate: 100

    protected override void HandleMissile(bool input) {
        // flocking missiles -- or just many missiles with poor tracking
    }

    protected override void HandleShoot(bool input) {
        // charged shot
    }

    protected override void HandleAbility(bool input) {
        // EMP (will require defining a 'stun' method to call on enemies in range)
    }
}