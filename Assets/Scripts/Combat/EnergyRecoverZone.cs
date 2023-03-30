using System.Collections;
using UnityEngine;

public class EnergyRecoverZone : ZoneOfEffect<EnergizedEntity> {

    [Tooltip("Energy recovery per second")]
    [SerializeField] float _energyRecover;

    protected override void FixedUpdate() {
        base.FixedUpdate();
        foreach (var entity in _InRange) {
            entity.RecoverEnergy(_energyRecover * Time.fixedDeltaTime);
        }
    }
}