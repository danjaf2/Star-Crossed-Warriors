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

        //IF YOU WANT TO ADD DATA TO THE DICTIONARY, YOU DO NOT NEED TO do parent.parent.parent...SetData() anymore, now just use the root
        //root.SetData("Guy", new object());

        //Evaluation of state and apply transformation changes
        state = NodeState.FAILURE;
        return state;
    }

    }
