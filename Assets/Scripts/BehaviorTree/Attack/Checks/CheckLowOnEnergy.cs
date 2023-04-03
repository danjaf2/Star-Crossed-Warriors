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
        threshold = 10;
        
    }

    public override NodeState Evaluate()
    {
        EnergyRecoverZone recharge = (EnergyRecoverZone)GetData("Recharge");

        if(recharge != null)
        {
            state = NodeState.SUCCESS;
            return state;
        }
        else
        {
            if(referenceTree.TryGetComponent<EnergizedEntity>(out EnergizedEntity ety))
            {
                if (ety.GetEnergyPercentage() < threshold)
                {
                    state = NodeState.SUCCESS;
                    return state;
                }
            }
        }
        
        state = NodeState.FAILURE;
        return state;
    }
}
