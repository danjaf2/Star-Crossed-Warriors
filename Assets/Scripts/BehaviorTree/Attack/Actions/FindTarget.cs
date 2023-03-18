using AI;
using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
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
        List<Collider> validTargets = new List<Collider>();
        Debug.Log("Searching for target");
        Transform target = (Transform)GetData("target");
        if (target == null)
        {
           hitColliders = Physics.OverlapSphere(referenceTree.transform.position, range, mask);
            foreach (Collider collider in hitColliders)
            {
                if (collider.transform.CompareTag("Plane")|| collider.transform.CompareTag("Player"))
                {
                    validTargets.Add(collider);
                }
            }
        }
        else
        {
            state = NodeState.FAILURE;
            Debug.Log("Already have target");
            return state;
        }
        if (validTargets.Count == 0)
        {
            state = NodeState.FAILURE;
            Debug.Log("Nothing in area");
            return state;
            
        }
        else
        {
            root.SetData("target", hitColliders[0].transform);//We can develop how to choose our target later
            referenceTree.GetComponent<AIAgent>().TrackTarget(hitColliders[0].transform);
            Debug.Log(hitColliders[0].transform.name);
            Debug.Log("Found Target");
        }

        state = NodeState.SUCCESS;
        return state;
    }
}
