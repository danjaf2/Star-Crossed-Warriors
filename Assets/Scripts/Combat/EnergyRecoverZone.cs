using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class EnergyRecoverZone : ZoneOfEffect<EnergizedEntity> {

    [Tooltip("Energy recovery per second")]
    [SerializeField] float _energyRecover;

    public List<Waypoint> waypoints;
    public LayerMask waypointMask;

    private void Start()
    {

       Collider[]points = Physics.OverlapSphere(this.transform.position, _radius, waypointMask);
        foreach (Collider c in points)
        {
            if(c.GetComponent<Waypoint>().connectedTo.Count> 1)
            {
            waypoints.Add(c.GetComponent<Waypoint>());
            }
        }
        
    }

    protected override void FixedUpdate() {
        base.FixedUpdate();
        foreach (var entity in _InRange) {
            if (NetworkManager.Singleton.IsServer)
            {
                entity.RecoverEnergy(_energyRecover * Time.fixedDeltaTime);
            }
        }
    }
}