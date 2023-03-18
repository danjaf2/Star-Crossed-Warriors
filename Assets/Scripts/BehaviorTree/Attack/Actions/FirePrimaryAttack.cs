using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePrimaryAttack : Node
{
    // Start is called before the first frame update

    float goalRange = 100f;//must be likely changed later
    public FirePrimaryAttack(float range)
    {
        
        goalRange = range;
    }
    public FirePrimaryAttack()
    {
        goalRange = 100f;
    }

    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");
        if (target != null)
        {
            //Add check for ammo maybe?
            Debug.Log("Pew");
            state = NodeState.SUCCESS;
            return state;

        }
        state = NodeState.FAILURE;
        return state;
    }
}