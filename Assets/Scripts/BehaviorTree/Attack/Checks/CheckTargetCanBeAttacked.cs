using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTargetCanBeAttacked : Node
{
    // Start is called before the first frame update
    float attackAngleThreshold = 5f;
    float attackRange = 100f;//must be likely changed later
    LayerMask mask;
    public CheckTargetCanBeAttacked(float attackAngle, float range, LayerMask m)
    {
        attackAngleThreshold= attackAngle;
        attackRange= range; 
        mask = m;
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
            if(Vector3.Distance(target.position, referenceTree.transform.position) < attackRange)
            {
                //Is in shooting reticle?
                if (Vector3.Angle(referenceTree.transform.forward, toTarget) <= attackAngleThreshold)
                {
                    //Can be seen? (might need to add layer mask later)
                    if (Physics.Raycast(referenceTree.transform.position, toTarget, out RaycastHit hit, Mathf.Infinity, mask))
                    {
                        //Debug.Log("Hit");
                        if (hit.transform.gameObject == target.gameObject)
                        {
                            //Debug.Log("Can fire");
                            state = NodeState.SUCCESS;
                            return state;
                        }
                    }
                }
            }
            
        }
        //Debug.Log("Cannot fire");
        //Evaluation of state and apply transformation changes
        state = NodeState.FAILURE;
        return state;
    }
}
