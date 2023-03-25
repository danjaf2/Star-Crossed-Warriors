using System.Collections;
using UnityEngine;

public class EnergyRecoverZone : ZoneOfEffect<ShipEntity> {

    [Tooltip("Energy recovery per second")]
    [SerializeField] float _energyRecover;

    protected override void FixedUpdate() {
        base.FixedUpdate();
        foreach (var ship in _InRange) {
            ship.RecoverEnergy(_energyRecover * Time.fixedDeltaTime);
        }
    }
}