using AI;
using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WanderTowardObjective : Node
{
    

    List<GameObject> objectives;

    float range = 600f;//must be likely changed later
    LayerMask mask;
    


    public WanderTowardObjective(float range, LayerMask mask)
    {
        this.range = range;
        this.mask = mask;
        objectives = ObjectiveManager.Instance.GetObjectiveList();

    }
    public WanderTowardObjective()//Dont use ever
    {
        objectives = ObjectiveManager.Instance.GetObjectiveList();
    }

    public override NodeState Evaluate()
    {

        
        if (referenceTree.TryGetComponent<ScoutShip>(out ScoutShip scout))
        {
            if (referenceTree.TryGetComponent<PlayerShip>(out PlayerShip ship))
            {
                if (referenceTree.gameObject.GetComponent<AIAgent>().goalWaypoint != null)
                {
                    if (Vector3.Dot((referenceTree.transform.forward).normalized, (referenceTree.gameObject.GetComponent<AIAgent>().goalWaypoint.transform.position - referenceTree.transform.position).normalized) > 0.98f)
                    {
                        RaycastHit hit;
                        LayerMask mask = LayerMask.GetMask("Obstacle");
                        if (!Physics.Raycast(referenceTree.transform.position, referenceTree.transform.forward, out hit, 2000, mask))
                        {
                            ship.SetAbilityInput(true);
                        }
                    }
                }
            }
        }

        if (((SimpleObjective)GetData("CurrentObjective")) != null)
        {
            
            if (GetData("CurrentObjective") != null)
            {
                if (Vector3.Distance(referenceTree.transform.position, ((SimpleObjective)root.GetData("CurrentObjective")).gameObject.transform.position) < 500|| Vector3.Distance(referenceTree.transform.position, ((SimpleObjective)root.GetData("CurrentObjective")).gameObject.transform.position) > 2500)
                {
                    root.ClearData("CurrentObjective");
                    state = NodeState.FAILURE;
                    return state;
                }
            }
        }
        Transform target = (Transform)GetData("target");
        //Debug.Log(referenceTree.gameObject.name);
        if (target == null && GetData("Recharge") == null)
        {
            if (referenceTree.TryGetComponent<AIAgent>(out AIAgent agent))
            {
                if(agent.goalWaypoint == null) {
                    Collider[] hitColliders = Physics.OverlapSphere(referenceTree.transform.position, range, mask);
                    if (hitColliders.Length > 0)
                    {
                       
                        agent.mostRecentWaypoint = GetClosestWaypoint(hitColliders).GetComponent<Waypoint>();
                        SimpleObjective zone = null;
                        if ((SimpleObjective)root.GetData("CurrentObjective") == null)
                        {
                             zone = findActiveObjective();
                            root.SetData("CurrentObjective", zone);
                        }
                        if (zone != null && Vector3.Distance(referenceTree.transform.position, ((SimpleObjective)root.GetData("CurrentObjective")).gameObject.transform.position)>=500)
                        {
                            //Debug.Log("Wandering Towards objective");
                            int randomNumber = Random.Range(0, zone.waypoints.Count - 1);
                            agent.goalWaypoint = zone.waypoints[randomNumber];
                            state = NodeState.SUCCESS;
                            return state;
                        }
                    }
                    else
                    {
                        agent.goalWaypoint = agent.mostRecentWaypoint;
                        state = NodeState.SUCCESS;
                        return state;
                    }
                }
            }
        }
        state = NodeState.FAILURE;
        return state;
    }

    SimpleObjective findActiveObjective()
    {
        objectives = ObjectiveManager.Instance.GetObjectiveList();
        foreach (GameObject objective in objectives)
        {
            if (objective.GetComponent<SimpleObjective>().isActiveObjective)
            {
                return objective.GetComponent<SimpleObjective>();
            }
        }
        return null;
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
