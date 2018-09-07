using System;
using System.Collections.Generic;

public enum DebuffType
{
    Stun,
    Drench,
    Knockback,
    Slow
}

public enum BuffType
{
    Invinciblity
}

public class StatusEffect
{
    public StatusEffect(bool isAffect = false, float duration = 0f, float strength = 0f)
    {
        IsAffected = isAffect;
        Duration = duration;
        Strength = strength;
    }

    public Func<float> EffectMethod { get; set; }
    public bool IsAffected { get; set; }
    public float Duration { get; set; }
    public float Strength { get; set; }
}

public class Buff : StatusEffect
{
    public Buff(bool isAffect = false, float duration = 0)
        : base(isAffect, duration) {}
}

public class Debuff : StatusEffect
{
    public Debuff(bool isAffect = false, float duration = 0) 
        : base(isAffect, duration) {}
}
