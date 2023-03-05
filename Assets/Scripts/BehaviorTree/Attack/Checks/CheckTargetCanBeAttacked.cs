using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTargetCanBeAttacked : Node
{
    // Start is called before the first frame update
    float attackAngleThreshold = 5f;
    float attackRange = 100f;//must be likely changed later
    public CheckTargetCanBeAttacked(float attackAngle, float range)
    {
        attackAngleThreshold= attackAngle;
        attackRange= range; 
    }
    public CheckTargetCanBeAttacked()
    {
        attackAngleThreshold = 5f;
        attackRange = 100f;
    }

    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");
        if (target != null)
        {
            Vector3 toTarget = target.position - referenceTree.transform.position;
            //Is in range?
            if(toTarget.magnitude < attackRange)
            {
                //Is in shooting reticle?
                if (Vector3.Angle(referenceTree.transform.forward, toTarget) <= attackAngleThreshold)
                {
                    //Can be seen? (might need to add layer mask later)
                    if (Physics.Raycast(referenceTree.transform.position, toTarget, out RaycastHit hit))
                    {
                        if (hit.transform.root == target)
                        {
                            state = NodeState.SUCCESS;
                            return state;
                        }
                    }
                }
            }
            
        }
        //Evaluation of state and apply transformation changes
        state = NodeState.FAILURE;
        return state;
    }
}
