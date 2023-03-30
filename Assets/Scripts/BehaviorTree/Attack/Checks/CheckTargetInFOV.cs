using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTargetInFOV : Node
{
    // Start is called before the first frame update
    float viewAngle = 120f;
    public CheckTargetInFOV()
    {
        //Constructor for setting up base values in Example Character Tree
    }

    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");
        if (target != null)
        {
            Vector3 toTarget = target.position - referenceTree.transform.position;
            if (Vector3.Angle(referenceTree.transform.forward, toTarget) <= viewAngle)
            {
                if (Physics.Raycast(referenceTree.transform.position, toTarget, out RaycastHit hit))
                {
                    if(hit.transform.gameObject == target.gameObject)
                    {
                        state = NodeState.SUCCESS;
                        return state;
                    }
                }
            }
        }
        //Evaluation of state and apply transformation changes
        state = NodeState.FAILURE;
        return state;
    }
}
