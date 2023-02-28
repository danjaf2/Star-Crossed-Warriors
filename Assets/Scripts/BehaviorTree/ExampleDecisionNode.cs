using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleDecisionNode : Node
{
    // Start is called before the first frame update
    public ExampleDecisionNode()
    {
         //Constructor for setting up base values in Example Character Tree
    }

    public override NodeState Evaluate()
    {
        //Evaluation of state and apply transformation changes



        state = NodeState.FAILURE;
        return state;
    }

    }
