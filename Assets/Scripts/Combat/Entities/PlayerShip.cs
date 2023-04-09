using UnityEngine;

public abstract class PlayerShip : ShipEntity {

    bool _shootInput;
    bool _missileInput;
    bool _abilityInput;

    //public bool playerControlled; 
    public void SetShootInput(bool value) { _shootInput = value; }
    public void SetMissileInput(bool value) { _missileInput = value; }
    public void SetAbilityInput(bool value) { _abilityInput = value; }


    public abstract void HandleShoot(bool input);
    public abstract void HandleMissile(bool input);
    public abstract void HandleAbility(bool input);



    #region UNITY LIFETIME

    protected override void FixedUpdate() {

        if (isStunned)
        {
            return; 
        }

        base.FixedUpdate();
        HandleShoot(_shootInput);
        HandleMissile(_missileInput);
        HandleAbility(_abilityInput);
    }

    private void Update()
    {
        if (isStunned)
        {
            _stunTimer += Time.deltaTime; 
        }

        if(_stunTimer > 2f) //Reset stun after 2 seconds
        {
            isStunned = false;
            _stunTimer = 0; 
        }
    }

    #endregion


    #region EMPSTUN
    [SerializeField] public bool isStunned = false;
    float _stunTimer = 0f; 

    public void BeStunned()
    {
        isStunned = true;
        _stunTimer = 0;

        //Deal shield damage

    }
    #endregion
}
