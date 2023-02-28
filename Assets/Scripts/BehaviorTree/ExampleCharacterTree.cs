using System.Collections.Generic;
using BehaviorTree;

public class ExampleCharacterTree : Tree
{
    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node>
        {
            
        }) ;

        return root;
    }
}
