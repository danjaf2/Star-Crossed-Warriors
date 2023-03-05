using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShip : Entity {


    [SerializeField] int _maxHealth = 150;
    [SerializeField] int _maxEnergy = 300;

    public int Health { get => _health; }
    int _health;
    public int Energy { get => _energy; }
    int _energy;


    Vector3 _direction;
    Vector3 _velocity;

    public void Maneuver(Maneuver maneuver) {
        throw new System.NotImplementedException();
    }

    #region INSTANTIATION

    public static PlayerShip CreateNew(int maxHP, int maxEnergy, Vector3 position) {
        GameObject spawned = new GameObject(nameof(PlayerShip));
        spawned.transform.position = position;
        PlayerShip instance = spawned.AddComponent<PlayerShip>();

        instance._maxHealth = maxHP;
        instance._maxEnergy = maxEnergy;

        instance.Setup();

        return instance;
    }

    public static PlayerShip CreateFromPrefab(PlayerShip prefab, Vector3 position) {
        PlayerShip instance = Instantiate(prefab, position, Quaternion.identity);
        return instance;
    }

    /// <summary>
    /// Perform whatever needed step is required on instantiation.
    /// </summary>
    void Setup() {

    }

    #endregion

    #region UNITY LIFETIME

    private void Awake() {
        
    }

    protected override void FixedUpdate() {
        base.FixedUpdate();
    }

    #endregion

}
