using System.Collections;
using UnityEngine;

public class EnergizedEntity : Entity {

    [SerializeField] int _maxEnergy = 300;

    public float Energy { get => _energy; }
    float _energy;

    protected override void Awake() {
        base.Awake();
        _energy = _maxEnergy;
    }
    
    public void RecoverEnergy(float amount) {
        _energy += amount;
        if (_energy > _maxEnergy) {
            _energy = _maxEnergy;
        }
    }

    public float GetEnergyPercentage()
    {
        return (_energy/_maxEnergy)*100;
    }
}