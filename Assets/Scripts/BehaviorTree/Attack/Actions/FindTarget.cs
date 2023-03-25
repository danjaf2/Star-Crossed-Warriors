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
    bool friendly =false;
    public FindTarget(float range, LayerMask mask, bool friendly)
    {
        this.range = range;
        this.mask = mask;
        this.friendly = friendly;
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
        if (target ==null)
        {
           hitColliders = Physics.OverlapSphere(referenceTree.transform.position, range, mask);
            foreach (Collider collider in hitColliders)
            {
                if (!friendly)
                {
                    if (collider.transform.CompareTag("Plane") || collider.transform.CompareTag("Player"))
                    {
                        validTargets.Add(collider);
                    }
                }
                else
                {
                    if (collider.transform.CompareTag("Enemy"))
                    {
                        validTargets.Add(collider);
                    }
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
            Transform t = GetClosestTarget(validTargets).transform;//He currently hyper fixates on one target, we should have a solution to make him switch maybe
            root.SetData("target", t);
            referenceTree.GetComponent<AIAgent>().TrackTarget(t);
            //Debug.Log(validTargets[0].transform.name);
            Debug.Log("Found Target");
        }

        state = NodeState.SUCCESS;
        return state;
    }

    Collider GetClosestTarget(List<Collider> targets)
    {
        Collider tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = referenceTree.transform.position;
        foreach (Collider t in targets)
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
