using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTargetInRange : Node
{
    // Start is called before the first frame update
    float range = 100f;

    public CheckTargetInRange(float desiredRange)
    {
        range = desiredRange;
    }
    public CheckTargetInRange()
    {
        range = 100;
        
    }

    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");
        if (target != null)
        {

            //TODO:INSERT CODE TO MAKE THE TREEREFERENCE MOVE TO TARGET


            Vector3 toTarget = target.position - referenceTree.transform.position;
            //Is in range?
            if (toTarget.magnitude < range)
            {
                state = NodeState.SUCCESS;
                return state;

            }
            else
            {
                state = NodeState.FAILURE;
                return state;
            }

        }
        state = NodeState.FAILURE;
        return state;
    }
}
