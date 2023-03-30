using AI;
using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class WanderRandomly : Node
{
    // Start is called before the first frame update
    Waypoint[] wp;
    List<Waypoint> waypoints;

    float range = 600f;//must be likely changed later
    LayerMask mask;

    public WanderRandomly(float range, LayerMask mask)
    {
        this.range = range;
        this.mask = mask;
        waypoints = GameObject.FindObjectsOfType(typeof(Waypoint)).Cast<Waypoint>().Where(obj => obj.connectedTo.Count>1).ToList();

    }
    public WanderRandomly()//Dont use ever
    {
        //Constructor for setting up base values in Example Character Tree
        waypoints = GameObject.FindObjectsOfType(typeof(Waypoint)).Cast<Waypoint>().Where(obj => obj.connectedTo.Count > 1).ToList();
    }

    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");
        //Debug.Log(referenceTree.gameObject.name);
        if (target == null)
        {
            if (referenceTree.TryGetComponent<AIAgent>(out AIAgent agent))
            {
                if (agent.goalWaypoint == null)
                {
                    Collider[] hitColliders = Physics.OverlapSphere(referenceTree.transform.position, range, mask);
                    //Select closest hit waypoint
                    if (hitColliders.Length > 0)
                    {
                        agent.mostRecentWaypoint = GetClosestWaypoint(hitColliders).GetComponent<Waypoint>();

                        int randomNumber = Random.Range(0, waypoints.Count - 1);
                        agent.goalWaypoint = waypoints[randomNumber];
                        state = NodeState.SUCCESS;
                        return state;
                    }
                    else
                    {
                        agent.goalWaypoint = agent.mostRecentWaypoint;
                    }
                }
            }
        }
        //Evaluation of state and apply transformation changes
        state = NodeState.FAILURE;
        return state;
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