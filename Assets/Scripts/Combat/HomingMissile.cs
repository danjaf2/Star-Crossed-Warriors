using System.Collections;
using UnityEngine;

public class HomingMissile : Entity {

    [SerializeField] float _turnSpeed;
    [SerializeField] float _speed;
    Entity _target;

    protected override void FixedUpdate() {
        base.FixedUpdate();
    }

    public static HomingMissile CreateFromPrefab(HomingMissile prefab, Vector3 position, Quaternion rotation, Entity target) {
        HomingMissile missile = Instantiate(prefab, position, rotation);
        missile._target = target;
        return missile;
    }
}