﻿using Quantum.QuantumUser.Simulation.Gameplay.Features.Statuses.Factory;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Statuses.Applier
{
    public unsafe class StatusApplier : IStatusApplier
    {
        private readonly IStatusFactory _statusFactory;

        public StatusApplier(IStatusFactory statusFactory)
        {
            _statusFactory = statusFactory;
        }

        public EntityRef ApplyStatus(Frame frame, StatusSetup setup, EntityRef producer, EntityRef target)
        {
            EntityRef status = new EntityRef();
            bool statusFounded = false;
            
            ComponentFilter<StatusTypeId, TargetId> componentFilter = frame.Filter<StatusTypeId, TargetId>();

            while (componentFilter.NextUnsafe(
                       out EntityRef entity, 
                       out StatusTypeId* statusTypeId,
                       out TargetId* targetId))
            {
                if (targetId->Value == target && statusTypeId->Value == setup.StatusTypeId)
                {
                    status = entity;
                    statusFounded = true;
                    break;
                }
            }

            if (statusFounded)
                frame.Set(status, new TimeLeft { Value = setup.Duration });
            else
                status = _statusFactory.CreateStatus(frame, setup, producer, target,
                    frame.Get<Owner>(producer));

            return status;
        }
    }
}