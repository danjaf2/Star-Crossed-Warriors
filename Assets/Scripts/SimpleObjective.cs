using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SimpleObjective : MonoBehaviour, IObjective {
    // This component is meant to be a simple way to make something into an objective.
    // Ex: Mothership is has a SimpleObjective component on it. On death, it calls the the CompleteObjective method.

    public string ObjectiveText = "SAMPLE OBJECTIVE";
    public string ObjectiveDescription => ObjectiveText;
    public GameObject Reference => this.gameObject;
    public bool active = false;
    public bool isActiveObjective { get => active; set => active = value; }

    public event Action<IObjective> OnObjectiveComplete;

    public List<Waypoint> waypoints;
    public LayerMask waypointMask;
    public float objectiveRadius = 1000;

    



    private void Awake() {
        ObjectiveManager.Instance.RegisterObjective(this);
    }

    private void Start()
    {
        Collider[] points = Physics.OverlapSphere(this.transform.position, objectiveRadius, waypointMask);
        foreach (Collider c in points)
        {
            //print(c);
            if (c.GetComponent<Waypoint>().connectedTo.Count > 1)
            {
                waypoints.Add(c.GetComponent<Waypoint>());
            }
        }
    }

    public void CompleteObjective() {
        OnObjectiveComplete(this);
    }
}