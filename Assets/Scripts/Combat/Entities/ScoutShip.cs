using System.Collections;
using UnityEngine;

public class ScoutShip : PlayerShip {

    //maxHealth: 150
    //maxEnergy: 300
    //speed: 2 (fast)
    //primaryFireRate: (medium)
    //lockOnRate: 100

    [SerializeField] HomingMissile _missilePrefab;

    protected override void HandleAbility(bool input) {
        // boost
    }

    protected override void HandleShoot(bool input) {
        // burst fire / shotgun
    }

    protected override void HandleMissile(bool input) {
        // strong homing missile

        // on release
        //HomingMissile.CreateFromPrefab(_missilePrefab, this.transform.position, this.transform.rotation, target);
    }
}