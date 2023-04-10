using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class ContinuousDamageZone : ZoneOfEffect<Entity> {

    [Tooltip("Energy recovery per second")]
    [SerializeField] float _damagePerSecond;

    [SerializeField] int _lifetime;

    protected override void FixedUpdate() {
        base.FixedUpdate();

        if(_lifetime-- <= 0) { Destroy(this.gameObject); }

        foreach (var entity in _InRange) {
            if (NetworkManager.Singleton.IsServer) {
            entity.Hit(new Attack(_damagePerSecond * Time.fixedDeltaTime, null));
            }
        }
    }
}