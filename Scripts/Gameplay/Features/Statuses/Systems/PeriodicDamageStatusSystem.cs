﻿using Photon.Deterministic;
using Quantum.QuantumUser.Simulation.Common.Di;
using Quantum.QuantumUser.Simulation.Gameplay.Common.Time;
using Quantum.QuantumUser.Simulation.Gameplay.Features.Effects.Factory;
using UnityEngine;
using UnityEngine.Scripting;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Statuses.Systems
{
    [Preserve]
    public unsafe class PeriodicDamageStatusSystem : SystemMainThreadFilter<PeriodicDamageStatusSystem.Filter>
    {
        private IEffectFactory _effectsFactory;

        public override void OnInit(Frame f)
        {
            _effectsFactory = DI.Resolve<IEffectFactory>();
        }

        public override void Update(Frame f, ref Filter filter)
        {
            f.Unsafe.GetPointer<TimeSinceLastTick>(filter.Entity)->Value -= f.DeltaTime;

            if (filter.TimeSinceLastTick->Value <= 0)
            {
                f.Unsafe.GetPointer<TimeSinceLastTick>(filter.Entity)->Value = filter.Period->Value;

                var value = filter.EffectValue->Value;
                _effectsFactory
                    .CreateEffect(f,
                    new EffectSetup { EffectTypeId = EffectTypeId.Damage, Value = value },
                    filter.ProducerId->Value, filter.TargetID->Value, 
                    f.Get<Owner>(filter.Entity));
            }
        }

        public struct Filter
        {
            public EntityRef Entity;
            public Fire* Fire;
            public Status* Status;
            public Period* Period;
            public TimeSinceLastTick* TimeSinceLastTick;
            public EffectValue* EffectValue;
            public ProducerId* ProducerId;
            public TargetId* TargetID;
        }
    }
}
