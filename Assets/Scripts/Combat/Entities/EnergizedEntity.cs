using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class EnergizedEntity : Entity {

    [SerializeField] int _maxEnergy = 300;

    public float Energy { get => _energy.Value; set { _energy.Value = value; } }
    public NetworkVariable<float> _energy = new NetworkVariable<float>(100, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    protected override void Awake() {
        base.Awake();
        if (IsOwner)
        {
            _energy.Value = _maxEnergy;
        }
        else
        {
            SetEnergyServerRpc(_maxEnergy);
        }
    }

    public void Start()
    {
        if (IsOwner)
        {
            _energy.Value = _maxEnergy;
        }
        else
        {
            SetEnergyServerRpc(_maxEnergy);
        }
    }

    public void RecoverEnergy(float amount) {
        //print("Recover?");
        //print(_energy.Value);
        if (IsOwner)
        {
            _energy.Value += amount;
        
        if (_energy.Value > _maxEnergy) {
            _energy.Value = _maxEnergy;
        }
        }
        else
        {
            GainEnergyServerRpc(amount);
        }
    }
    private void Update()
    {
       
    }

    public float GetOwnEnergy()
    {
        return _energy.Value;
    }
        public void LoseEnergy(float amount)
    {
        if(IsOwner) { 
            _energy.Value = _energy.Value - amount;
            if (_energy.Value <= 0)
            {
                _energy.Value = 0;
            }
        }
        else
        {
            LoseEnergyServerRpc(amount);
        }
        
        
    }

    public float GetEnergyPercentage()
    {
            return (_energy.Value / _maxEnergy) * 100;
    }

    [ServerRpc(RequireOwnership = false)]
    private void LoseEnergyServerRpc(float amount)
    {
        _energy.Value -= amount;

        if (_energy.Value <= 0)
        {
            _energy.Value = 0;
        }
    }


    [ServerRpc(RequireOwnership = false)]
    private void GainEnergyServerRpc(float amount)
    {
        _energy.Value += amount;

        if (_energy.Value > _maxEnergy)
        {
            _energy.Value = _maxEnergy;
        }
    }

[ServerRpc(RequireOwnership = false)]
private void SetEnergyServerRpc(float amount)
{
    _energy.Value = amount;
}
}