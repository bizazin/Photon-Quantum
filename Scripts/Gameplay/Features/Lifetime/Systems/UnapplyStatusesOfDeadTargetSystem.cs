using ModestTree.Util;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Lifetime.Systems
{
    [Preserve]
    public unsafe class UnapplyStatusesOfDeadTargetSystem : SystemMainThread
    {
        public override void Update(Frame f)
        {
            var statuses = f.Filter<Status, TargetId>();
            var dead = f.Filter<Dead>();

            while (statuses.NextUnsafe(
                       out EntityRef status,
                       out _,
                       out TargetId* targetId))
            while (dead.NextUnsafe(
                       out EntityRef entity,
                       out _))
            {
                if (targetId->Value == entity) 
                    f.Add<Unapplied>(status);
            }
        }
    }
}