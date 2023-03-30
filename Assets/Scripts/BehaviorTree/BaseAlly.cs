using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

public class BaseAlly: BehaviorTree.Tree
{
    public LayerMask findMask;
    public float findRange = 700f;

    public float maxAggroRange = 800f;

    public float attackRange = 500;
    public float attackAngleThreshold = 10;
    public LayerMask attackMask;

    public float wanderStartNodeSearchRange;
    public LayerMask wanderMask;

    public bool friendly;

    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node>
        {
            new Selector(new List<Node>
            {
                new CheckAllyIsInDanger(),
                new Sequence(new List<Node>{ 
                new CheckTargetCanBeAttacked(attackAngleThreshold, attackRange, attackMask),
                new Sequence(new List<Node>{
                    new FirePrimaryAttack(),
                     new Sequence(new List<Node>{
                   //Secondary attack stuff here
                })

                })


                }), new Sequence(new List<Node>
                {
                    new CheckTargetInFOV(),
                    new FollowTarget(maxAggroRange)
                }),
                new FindTarget(findRange, findMask, friendly)

            }),
            new Selector(new List<Node>
            { 
                
                new WanderRandomly(wanderStartNodeSearchRange, wanderMask)
            })
        }); ;
        
        setRoot(root.children, root);
        setTreeRef(root.children, this);
        return root;
    }
}
