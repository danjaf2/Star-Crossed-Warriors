using AI;
using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : Node
{
    // Start is called before the first frame update

    float goalRange = 500f;//must be likely changed later
    public FollowTarget(float range)
    {
        
        goalRange = range;
    }
    public FollowTarget()
    {
        goalRange = 500f;
    }

    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");
        if (target != null)
        {
            
            //TODO:INSERT CODE TO MAKE THE TREEREFERENCE TO COMMAND AGENT TO GET CLOSER TO THE TARGET
            if(Vector3.Distance(target.position, referenceTree.transform.position)<goalRange) {
                referenceTree.GetComponent<AIAgent>().TrackTarget(target);
            }
            else
            {
                root.ClearData("target");
                referenceTree.GetComponent<AIAgent>().TrackTarget(null);
            }
            


            
            state = NodeState.SUCCESS;
            return state;
            //Is in range?
            /*
            if (toTarget.magnitude < goalRange)
            {
                state = NodeState.SUCCESS;
                return state;

            }
            else
            {
                state = NodeState.RUNNING;
                return state;
            }
            */

        }
        state = NodeState.FAILURE;
        return state;

    }
}
