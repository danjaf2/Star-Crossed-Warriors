using AI;
using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class WanderNearStar : Node
{
    // Start is called before the first frame update
    // Waypoint[] wp;
    List<EnergyRecoverZone> lights;

    float GoalThreshold = 90f;

    public WanderNearStar(float threshold)
    {
        this.GoalThreshold = threshold;
        lights = GameObject.FindObjectsOfType(typeof(EnergyRecoverZone)).Cast<EnergyRecoverZone>().Where(obj => obj.waypoints.Count > 0).ToList();
    }
    public WanderNearStar()//Dont use ever
    {
        //Constructor for setting up base values in Example Character Tree
        lights = GameObject.FindObjectsOfType(typeof(EnergyRecoverZone)).Cast<EnergyRecoverZone>().Where(obj => obj.waypoints.Count > 0).ToList();
    }

    public override NodeState Evaluate()
    {
        EnergyRecoverZone target = (EnergyRecoverZone)GetData("Recharge");
        //Debug.Log(referenceTree.gameObject.name);
        if (target == null)
        {
            if (referenceTree.TryGetComponent<AIAgent>(out AIAgent agent))
            {
                agent.goalWaypoint = null;
                agent.UnTrackTarget();
            }
            target = GetClosestStar();
            root.SetData("Recharge", target);
        }
        if (target != null)
        {
            if (referenceTree.TryGetComponent<AIAgent>(out AIAgent agent))
            {
                if (referenceTree.TryGetComponent<EnergizedEntity>(out EnergizedEntity ety))
                {
                    if (ety.GetEnergyPercentage() >= GoalThreshold)
                    {
                        root.ClearData("Recharge");
                        state = NodeState.FAILURE;
                        return state;
                    }
                }

                if (agent.goalWaypoint == null)
                {
                int randomNumber = Random.Range(0, target.waypoints.Count - 1);
                agent.goalWaypoint = target.waypoints[randomNumber];
                state = NodeState.SUCCESS;
                return state;
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
}