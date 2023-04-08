using AI;
using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WanderByStar : Node
{
    

    List<EnergyRecoverZone> lights;

    float range = 600f;//must be likely changed later
    LayerMask mask;


    public WanderByStar(float range, LayerMask mask)
    {
        this.range = range;
        this.mask = mask;
        lights = GameObject.FindObjectsOfType(typeof(EnergyRecoverZone)).Cast<EnergyRecoverZone>().Where(obj => obj.waypoints.Count > 0).ToList();

    }
    public WanderByStar()//Dont use ever
    {
        lights = GameObject.FindObjectsOfType(typeof(EnergyRecoverZone)).Cast<EnergyRecoverZone>().Where(obj => obj.waypoints.Count > 0).ToList();
    }

    public override NodeState Evaluate()
    {

        if (lights.Count == 0)
        {
            lights = GameObject.FindObjectsOfType(typeof(EnergyRecoverZone)).Cast<EnergyRecoverZone>().Where(obj => obj.waypoints.Count > 0).ToList();
        }

        if(referenceTree.TryGetComponent<EnergizedEntity>(out EnergizedEntity ety))
        {
            if (ety.GetEnergyPercentage() >= 60)
            {
                state = NodeState.FAILURE;
                return state;
            }
        }
        Transform target = (Transform)GetData("target");
        //Debug.Log(referenceTree.gameObject.name);
        if (target == null && GetData("Recharge") == null)
        {
            if (referenceTree.TryGetComponent<AIAgent>(out AIAgent agent))
            {
                if (agent.goalWaypoint == null)
                {
                    Collider[] hitColliders = Physics.OverlapSphere(referenceTree.transform.position, range, mask);
                    if (hitColliders.Length > 0)
                    {
                        Debug.Log("Wandering Towards star");
                        agent.mostRecentWaypoint = GetClosestWaypoint(hitColliders).GetComponent<Waypoint>();

                        EnergyRecoverZone zone = GetClosestStar();
                        int randomNumber = Random.Range(0, zone.waypoints.Count - 1);
                        agent.goalWaypoint = zone.waypoints[randomNumber];
                        state = NodeState.SUCCESS;
                        return state;
                    }
                    else
                    {
                        agent.goalWaypoint = agent.mostRecentWaypoint;
                        state = NodeState.FAILURE;
                        return state;
                    }
                }
            }
        }
        state = NodeState.FAILURE;
        return state;
    }

    EnergyRecoverZone GetClosestStar()
    {
        EnergyRecoverZone tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = referenceTree.transform.position;
        foreach (EnergyRecoverZone t in lights)
        {
            float dist = Vector3.Distance(t.transform.position, currentPos);
            if (dist < minDist)
            {
                tMin = t;
                minDist = dist;
            }
        }
        return tMin;
    }

    Collider GetClosestWaypoint(Collider[] points)
    {
        Collider tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = referenceTree.transform.position;
        foreach (Collider t in points)
        {
            float dist = Vector3.Distance(t.transform.position, currentPos);
            if (dist < minDist)
            {
                tMin = t;
                minDist = dist;
            }
        }
        return tMin;
    }
}
