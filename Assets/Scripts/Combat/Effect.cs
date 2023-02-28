using System.Collections;
using UnityEngine;


public class Effect {

    public Effect(Entity affecting) {
        _target = affecting;
    }

    public virtual int Priority => 0;
    public virtual string Name => "Default Effect";

    Entity _target;

    public virtual Attack OnHit(Attack toModify) {
        return toModify;
    }
}