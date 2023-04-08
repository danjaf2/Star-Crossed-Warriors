using UnityEngine;

public abstract class PlayerShip : ShipEntity {

    bool _shootInput;
    bool _missileInput;
    bool _abilityInput;
    public void SetShootInput(bool value) { _shootInput = value; }
    public void SetMissileInput(bool value) { _missileInput = value; }
    public void SetAbilityInput(bool value) { _abilityInput = value; }


    public abstract void HandleShoot(bool input);
    public abstract void HandleMissile(bool input);
    public abstract void HandleAbility(bool input);



    #region UNITY LIFETIME

    protected override void FixedUpdate() {
        base.FixedUpdate();
        HandleShoot(_shootInput);
        HandleMissile(_missileInput);
        HandleAbility(_abilityInput);
    }

    #endregion

}
