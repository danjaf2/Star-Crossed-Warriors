using UnityEngine;

public abstract class PlayerShip : ShipEntity {

    public float _flightSpeed;
    public float _fireRate;
    public float _lockOnDelay;


    bool _shootInput;
    bool _missileInput;
    bool _abilityInput;

    void SetShootInput(bool value) { _shootInput = value; }
    void SetMissileInput(bool value) { _missileInput = value; }
    void SetAbilityInput(bool value) { _abilityInput = value; }


    protected abstract void HandleShoot(bool input);
    protected abstract void HandleMissile(bool input);
    protected abstract void HandleAbility(bool input);



    #region UNITY LIFETIME

    protected override void Awake() {
        base.Awake();

        // Try to subsribe to controller events.
        if(this.TryGetComponent<ShipController>(out var controller)) {
            controller.GetShootInput += SetShootInput;
            controller.GetMissileInput += SetMissileInput;
            controller.GetAbilityInput += SetAbilityInput;
        }
        else { Debug.LogWarning(this.name + " has no controller attached!", this); }
    }


    protected override void FixedUpdate() {
        base.FixedUpdate();
        HandleShoot(_shootInput);
        HandleMissile(_missileInput);
        HandleAbility(_abilityInput);
    }

    #endregion

}
