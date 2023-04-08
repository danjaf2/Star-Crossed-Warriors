using AI;
using System.Collections;
using UnityEngine;


public class Effect {
    int _remainingDuration;

    public Effect(Entity affecting) {
        _Target = affecting;
        _Target.OnTick += Tick;
        _remainingDuration = Duration;
    }

    public virtual int Priority => 0;
    public virtual int Duration => 500;
    public virtual bool Stacks => true;
    public virtual string Name => "Default Effect";

    protected Entity _Target;

    public virtual void ModifyHit(Attack toModify) { }

    public virtual void Tick() {
        if(_remainingDuration-- <= 0) {
            _Target.RemoveEffect(this);   
        }
    }

    public void ResetDuration() { _remainingDuration = Duration; }
    public static int PrioritySort(Effect x, Effect y) => x.Priority - y.Priority;
}

public class FragileEffect : Effect {
    public override int Duration => 750;
    public override bool Stacks => false;

    float _dmgMultiplier;
    public FragileEffect(Entity affecting, float dmgMultiplier = 2) : base(affecting) {
        _dmgMultiplier = dmgMultiplier;
    }

    public override void ModifyHit(Attack toModify) {
        toModify.Damage = (int)(toModify.Damage * _dmgMultiplier);
    }

    // A static function structured like this can be directly subscribed to an Attack's 'OnHit' event.
    public static void ApplyEffect(Attack atk, Entity applyTo) {
        applyTo.AddEffect(new FragileEffect(applyTo));
    }
}

public class ResetAggroEffect : Effect
{
    public override int Duration => 1;
    public override bool Stacks => false;

    float _dmgMultiplier;
    public ResetAggroEffect(Entity affecting) : base(affecting)
    {
        if (affecting.gameObject.TryGetComponent<AIAgent>(out AIAgent agent))
        {
            if (affecting.gameObject.TryGetComponent<BehaviorTree.Tree>(out BehaviorTree.Tree tree))
            {
                
                tree._root.ClearData("target");
                agent.UnTrackTarget();
            }

        }
    }

    public override void ModifyHit(Attack toModify)
    {
       
    }

    public void resetAggro(Attack atk,Entity affecting)
    {
        

    }

    // A static function structured like this can be directly subscribed to an Attack's 'OnHit' event.
    public static void ApplyEffect(Attack atk, Entity applyTo)
    {
        applyTo.AddEffect(new ResetAggroEffect(applyTo));
    }
}

public class BurningEffect : Effect {
    float _dmgPerSecond;
    
    public BurningEffect(Entity affecting, float dmgPerTick = 1f) : base(affecting) {
        _dmgPerSecond = dmgPerTick;
    }

    public override void Tick() {
        _Target.Hit(new Attack(_dmgPerSecond * Time.fixedDeltaTime, null));
    }
}