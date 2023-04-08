using AI;
using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : Node
{
    // Start is called before the first frame update

    float goalRange = 500f;//must be likely changed later
    public FollowTarget(float range)
    {
        
        goalRange = range;
    }
    public FollowTarget()
    {
        goalRange = 500f;
    }

    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");
        
        if (target != null && GetData("Recharge")==null)
        {
            
            //TODO:INSERT CODE TO MAKE THE TREEREFERENCE TO COMMAND AGENT TO GET CLOSER TO THE TARGET
            if(Vector3.Distance(target.position, referenceTree.transform.position)<goalRange && GetData("Recharge") == null) {
               
                referenceTree.GetComponent<AIAgent>().TrackTarget(target);

                if (referenceTree.TryGetComponent<ScoutShip>(out ScoutShip scout))
                {
                    if (referenceTree.TryGetComponent<PlayerShip>(out PlayerShip ship))
                    {

                        if (Vector3.Dot((referenceTree.transform.forward).normalized, (target.transform.position - referenceTree.transform.position).normalized) > 0.80f)
                        {
                            RaycastHit hit;
                            LayerMask mask = LayerMask.GetMask("Obstacle", "Character");
                            if (!Physics.Raycast(referenceTree.transform.position, referenceTree.transform.forward, out hit, 1000, mask))
                            {
                                ship.SetAbilityInput(true);
                            }
                        }
                    }
                }
            }
            else
            {
                root.ClearData("target");
                referenceTree.GetComponent<AIAgent>().UnTrackTarget();
            }

            




            state = NodeState.SUCCESS;
            return state;
            //Is in range?
            /*
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
            */

        }
        state = NodeState.FAILURE;
        return state;

    }
}
