using BehaviorTree;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckLowOnEnergy : Node
{
    // Start is called before the first frame update
    float threshold = 10f;

    public CheckLowOnEnergy(float threshold)
    {
        this.threshold = threshold;
    }
    public CheckLowOnEnergy()
    {
        threshold = 100;
        
    }

    public override NodeState Evaluate()
    {
        
        if (true)//CHANGE WHEN WE HAVE ENERGY IMPLEMENTED
        {
            state = NodeState.SUCCESS;
            return state;

        }
        state = NodeState.FAILURE;
        return state;
    }
}
