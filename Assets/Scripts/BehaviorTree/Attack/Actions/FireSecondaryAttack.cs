using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSecondaryAttack : Node
{
    // Start is called before the first frame update

    float goalRange = 100f;//must be likely changed later
    public FireSecondaryAttack(float range)
    {
        
        goalRange = range;
    }
    public FireSecondaryAttack()
    {
        goalRange = 100f;
    }

    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");
        if (target != null)
        {
            //Add check for ammo maybe?
            //Debug.Log("Pew");
            if(referenceTree.TryGetComponent<PlayerShip>(out PlayerShip ship))
            {
                if (referenceTree.TryGetComponent<ScoutShip>(out ScoutShip s))
                {
                    ship.SetMissileInput(true);
                }
            }
            else
            {
               Debug.Log("No PLayerShipComponent");
            }
            state = NodeState.SUCCESS;
            return state;

        }
        state = NodeState.FAILURE;
        return state;
    }
}
