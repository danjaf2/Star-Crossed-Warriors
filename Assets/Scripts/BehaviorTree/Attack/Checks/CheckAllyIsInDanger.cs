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

            if (referenceTree.TryGetComponent<ScoutShip>(out ScoutShip scout))
            {
                if (referenceTree.TryGetComponent<PlayerShip>(out PlayerShip ship))
                {
                   
                    if(Vector3.Dot((referenceTree.transform.forward).normalized, (((Waypoint)GetData("AllyPingedLocation")).gameObject.transform.position - referenceTree.transform.position).normalized) > 0.98f)
                    {
                        RaycastHit hit;
                        LayerMask mask = LayerMask.GetMask("Obstacle");
                        if (!Physics.Raycast(referenceTree.transform.position, referenceTree.transform.forward , out hit, 2000, mask))
                        {
                            ship.SetAbilityInput(true);
                        }
                    }
                }
            }
            if (Vector3.Distance(referenceTree.transform.position, target.transform.position) < referenceTree.GetComponent<BaseAlly>().findRange)
            {
                root.ClearData("AllyPingedLocation");
            }
            
            state = NodeState.SUCCESS;
            return state;
        }

    }
}
