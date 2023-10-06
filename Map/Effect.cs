using System;
using UnityEngine;

public abstract class Effect
{
    public float LifeTime { get; private set; }
    public float MaxLifeTime { get; private set; }

    public event Action<float, float> OnLifeTimeChanged;

    public event Action OnEnded;

    public abstract EffectName EffectName { get; }

    protected Effect(float lifeTime)
    {
        LifeTime = lifeTime;
        MaxLifeTime = lifeTime;
    }

    public virtual void Destroy()
    {
        OnEnded?.Invoke();
    }

    public virtual void Start()
    {
    }

    public virtual void AddLifeTime(float lifeTime)
    {
        LifeTime += lifeTime;
        if (LifeTime > MaxLifeTime)
        {
            MaxLifeTime = LifeTime;
        }
        OnLifeTimeChanged?.Invoke(LifeTime, MaxLifeTime);
    }

    public void SpendFixedDeltaTime()
    {
        LifeTime = MathF.Max(LifeTime - Time.fixedDeltaTime, 0);
        OnLifeTimeChanged?.Invoke(LifeTime, MaxLifeTime);
    }
}

public class SlowTimeEffect : Effect
{
    public SlowTimeEffect(float lifeTime) : base(lifeTime)
    {
    }

    public override void Start()
    {
        base.Start();
        Time.timeScale = 0.3f;
    }

    public override void Destroy()
    {
        base.Destroy();
        Time.timeScale = 1;
    }

    public override EffectName EffectName => EffectName.SlowDownTime;
}

public class InversionEffect : Effect
{
    public InversionEffect(float lifeTime) : base(lifeTime)
    {
    }

    public override EffectName EffectName => EffectName.Inversion;
}

public class SpeedTimeEffect : Effect
{
    public SpeedTimeEffect(float lifeTime) : base(lifeTime)
    {
    }

    public override EffectName EffectName => EffectName.SpeedUpTime;

    public override void Start()
    {
        base.Start();
        Time.timeScale = 3;
    }

    public override void Destroy()
    {
        base.Destroy();
        Time.timeScale = 1;
    }

    public override void AddLifeTime(float lifeTime)
    {
        base.AddLifeTime(lifeTime);
    }
}

public class AbilityToSwimEffect : Effect
{
    public AbilityToSwimEffect(float lifeTime) : base(lifeTime)
    {
    }

    public override EffectName EffectName => EffectName.SwimingAbility;
}