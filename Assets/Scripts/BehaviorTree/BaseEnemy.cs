using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

public class BaseEnemy: BehaviorTree.Tree
{
    public LayerMask findMask;
    public float findRange = 700f;

    public float maxAggroRange = 800f;

    public float attackRange = 500;
    public float attackAngleThreshold = 10;
    public LayerMask attackMask;
    public float projectileSpeed = 15000;
    public bool predict = true;

    public float wanderStartNodeSearchRange;
    public LayerMask wanderMask;

    public bool friendly;

    public float energyRequirementPercentThreashold=10;
    public float energyDesiredPercentageThreashold=90;

    

    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node>
        {
            
            new Selector(new List<Node>
            {
                new Sequence(new List<Node>{
                    new CheckLowOnEnergy(energyRequirementPercentThreashold),
                    new WanderNearStar(energyDesiredPercentageThreashold, wanderStartNodeSearchRange, wanderMask)
                }),
                new Sequence(new List<Node>{ 
                new CheckTargetCanBeAttacked(attackAngleThreshold, attackRange, attackMask, projectileSpeed, predict),
                new Sequence(new List<Node>{
                    new FirePrimaryAttack(),
                     new Sequence(new List<Node>{
                   //Secondary attack stuff here if any
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
                new WanderByStar(wanderStartNodeSearchRange, wanderMask),
                new WanderTowardObjective(wanderStartNodeSearchRange, wanderMask),
                new WanderRandomly(wanderStartNodeSearchRange, wanderMask)
            })
        }); ;
        
        setRoot(root.children, root);
        setTreeRef(root.children, this);
        return root;
    }
}
