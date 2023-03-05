using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : Node
{
    // Start is called before the first frame update

    float goalRange = 100f;//must be likely changed later
    public FollowTarget(float range)
    {
        
        goalRange = range;
    }
    public FollowTarget()
    {
        goalRange = 100f;
    }

    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");
        if (target != null)
        {

            //TODO:INSERT CODE TO MAKE THE TREEREFERENCE TO COMMAND AGENT TO GET CLOSER TO THE TARGET


            Vector3 toTarget = target.position - referenceTree.transform.position;
            //Is in range?
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

        }
        state = NodeState.FAILURE;
        return state;
    }
}
