using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllignWithTarget : Node
{
    public float goalAngle;
    public AllignWithTarget()
    {
       
    }
   
    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");
        if (target != null)
        {

            //TODO:INSERT CODE TO MAKE THE TREEREFERENCE TO COMMAND AGENT TO ALLIGN WITH TARGET


            Vector3 toTarget = target.position - referenceTree.transform.position;
            float angle = Vector3.Angle(toTarget, referenceTree.transform.forward);
            //Is in range?
            if (angle <= goalAngle)
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
