using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTargetCanBeAttacked : Node
{
    // Start is called before the first frame update
    float attackAngleThreshold = 5f;
    float attackRange = 100f;//must be likely changed later
    float projectileSpeed = 15000;
    LayerMask mask;
    bool predictiveAiming = true;
    public CheckTargetCanBeAttacked(float attackAngle, float range, LayerMask m, float projectilespeed, bool predictMovement)
    {
        attackAngleThreshold= attackAngle;
        attackRange= range; 
        mask = m;
        projectileSpeed = projectilespeed;
        predictiveAiming = predictMovement;
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
                Vector3 ev = Vector3.zero;
                if (predictiveAiming)
                {
                    ev = GetPredictedPoint(target.position, target.GetComponent<Rigidbody>().velocity, referenceTree.transform.position, projectileSpeed) - referenceTree.transform.position;
                }
                else
                {
                    ev = toTarget;
                }
                //Is in shooting reticle?

                //Can be seen? (might need to add layer mask later)
                if (Physics.Raycast(referenceTree.transform.position, toTarget, out RaycastHit hit, Mathf.Infinity, mask))
                {
                    if(target.gameObject.layer == 8)
                    {
                        if (Vector3.Angle(referenceTree.transform.forward, toTarget) <= 60)
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
                    if (Vector3.Angle(referenceTree.transform.forward, ev) <= attackAngleThreshold)
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

    Vector3 GetPredictedPoint(Vector3 targetPosition, Vector3 targetSpeed, Vector3 attackerPosition, float bulletSpeed)
    {//Quadratic formula
        Vector3 q = targetPosition - attackerPosition;
        q.y = 0;
        targetSpeed.y = 0;
        float a = Vector3.Dot(targetSpeed, targetSpeed) - (bulletSpeed * bulletSpeed);
        float b = 2 * Vector3.Dot(targetSpeed, q);
        float c = Vector3.Dot(q, q);
        Vector3 ret = targetPosition + targetSpeed;
        return ret;
    }
}
