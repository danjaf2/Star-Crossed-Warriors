using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindTarget : Node
{
    // Start is called before the first frame update

    float range = 300f;//must be likely changed later
    LayerMask mask;
    public FindTarget(float range, LayerMask mask)
    {
        this.range = range;
        this.mask = mask;
    }
    public FindTarget()
    {
        range = 300f;
    }

    public override NodeState Evaluate()
    {
        Collider[] hitColliders;
        Transform target = (Transform)GetData("target");
        if (target == null)
        {
           hitColliders = Physics.OverlapSphere(referenceTree.transform.position, range, mask);
        }
        else
        {
            state = NodeState.FAILURE;
            return state;
        }
        if (hitColliders.Length == 0)
        {
            state = NodeState.FAILURE;
            return state;
        }
        else
        {
            root.SetData("target", hitColliders[0]);//We can develop how to choose our target later
        }

        state = NodeState.SUCCESS;
        return state;
    }
}
