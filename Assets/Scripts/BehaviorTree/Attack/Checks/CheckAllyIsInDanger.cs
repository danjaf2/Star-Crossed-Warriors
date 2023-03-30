using AI;
using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CheckAllyIsInDanger : Node
{
    // Start is called before the first frame update
    List<Waypoint> waypoints;

    public CheckAllyIsInDanger()
    {
        waypoints = GameObject.FindObjectsOfType(typeof(Waypoint)).Cast<Waypoint>().Where(obj => obj.connectedTo.Count > 1).ToList();
    }
    

    public override NodeState Evaluate()
    {
        Waypoint target = (Waypoint)GetData("AllyPingedLocation");
        if (target == null)
        {
            state = NodeState.FAILURE;
            return state;
        }
        else
        {
            if(referenceTree.TryGetComponent<AIAgent>(out AIAgent agent))
            {
                if (agent.goalWaypoint != (Waypoint)GetData("AllyPingedLocation"))
                {
                    agent.goalWaypoint = target;
                }
            }
            if(Vector3.Distance(referenceTree.transform.position, target.transform.position) < 300)
            {
                root.ClearData("AllyPingedLocation");
            }
            
            state = NodeState.SUCCESS;
            return state;
        }

    }
}
