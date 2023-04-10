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
        
        EnergyRecoverZone recharge = (EnergyRecoverZone)root.GetData("Recharge");

        if(recharge != null)
        {
            if (referenceTree.TryGetComponent<EnergizedEntity>(out EnergizedEntity ety))
            {
                //Debug.Log(ety.GetEnergyPercentage());
            }

            state = NodeState.SUCCESS;
            return state;
        }
        else
        {
            if(referenceTree.TryGetComponent<EnergizedEntity>(out EnergizedEntity ety))
            {
                if (ety.GetEnergyPercentage() < threshold)
                {
                    Debug.Log(ety.GetEnergyPercentage());
                    state = NodeState.SUCCESS;
                    return state;
                }
            }
        }
        
        state = NodeState.FAILURE;
        return state;
    }
}
