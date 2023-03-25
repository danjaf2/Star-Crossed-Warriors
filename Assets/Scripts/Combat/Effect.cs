using System.Collections;
using UnityEngine;


public class Effect {
    int _remainingDuration;

    public Effect(Entity affecting) {
        _Target = affecting;
        _remainingDuration = Duration;
    }

    public virtual int Priority => 0;
    public virtual int Duration => 500;
    public virtual string Name => "Default Effect";

    protected Entity _Target;

    public virtual void ModifyHit(Attack toModify) { }

    public virtual void Tick() {
        if(_remainingDuration-- <= 0) {
            _Target.RemoveEffect(this);   
        }
    }

    public static int PrioritySort(Effect x, Effect y) => x.Priority - y.Priority;
}

public class FragileEffect : Effect {
    public override int Duration => 750;


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

public class BurningEffect : Effect {
    float _dmgPerSecond;
    
    public BurningEffect(Entity affecting, float dmgPerTick = 1f) : base(affecting) {
        _dmgPerSecond = dmgPerTick;
    }

    public override void Tick() {
        _Target.Hit(new Attack(_dmgPerSecond * Time.fixedDeltaTime, null));
    }
}