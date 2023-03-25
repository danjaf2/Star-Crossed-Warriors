using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShip : ShipEntity {


    public void Maneuver(Maneuver maneuver) {
        throw new System.NotImplementedException();
    }

    public override void Hit(Attack atk) {
        base.Hit(atk);
    }

    #region UNITY LIFETIME

    private void Awake() {
        
    }

    protected override void FixedUpdate() {
        base.FixedUpdate();
    }

    #endregion

}
