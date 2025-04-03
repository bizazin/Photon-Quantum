using ModestTree.Util;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Effects.Systems
{
    [Preserve]
    public unsafe class ProcessDamageEffectSystem : SystemMainThreadFilter<ProcessDamageEffectSystem.Filter>
    {
        public override void Update(Frame f, ref Filter filter)
        {
            EntityRef effect = filter.Entity;
            
            EntityRef? target = effect.Target(f);
            
            if (target == null)
                return;

            f.Add<Processed>(effect);

            if (f.Has<Dead>(target.Value))
                return;

            f.Unsafe.GetPointer<CurrentHp>(target.Value)->Value -= filter.EffectValue->Value;

            f.Events.DamageTaken(target.Value, -filter.EffectValue->Value);
        }

        public struct Filter
        {
            public EntityRef Entity;
            public DamageEffect* Effect;
            public EffectValue* EffectValue;
            public TargetId* TargetId;
        }
    }
}