using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class EnergizedEntity : Entity {

    [SerializeField] int _maxEnergy = 300;

    public float Energy { get => _energy.Value; set { _energy.Value = value; } }
    public NetworkVariable<float> _energy = new NetworkVariable<float>(100, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private float current;

    protected override void Awake() {
        _energy.Value = _maxEnergy;
        base.Awake();
    }

    public void Start()
    {
        if(NetworkManager.IsServer) {
            _energy.Value = _maxEnergy;
        }
    }

    public void RecoverEnergy(float amount) {
        //print("Recover?");
        //print(_energy.Value);
        if (NetworkManager.Singleton.IsServer)
        {
            _energy.Value += amount;

            if (_energy.Value > _maxEnergy)
            {
                _energy.Value = _maxEnergy;
            }
        } 
        
    }
    private void Update()
    {
       
    }

    public float GetOwnEnergy()
    {
        current = _energy.Value;
        return current;
    }
        public void LoseEnergy(float amount)
    {
        if(NetworkManager.Singleton.IsServer) { 
            _energy.Value = _energy.Value - amount;
            if (_energy.Value <= 0)
            {
                _energy.Value = 0;
            }
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