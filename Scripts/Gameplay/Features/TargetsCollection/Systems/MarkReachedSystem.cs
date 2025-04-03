using UnityEngine.Scripting;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.TargetsCollection.Systems
{
    [Preserve]
    public unsafe class MarkReachedSystem : SystemMainThread
    {

        public override void Update(Frame f)
        {
            var without = ComponentSet.Create<Reached>();
            var filter = f.Filter<TargetBuffer>(without);

            while (filter.NextUnsafe(
                       out EntityRef entity,
                       out TargetBuffer* targetBuffer
                   ))
            {
                if (f.ResolveList(targetBuffer->Value).Count > 0) 
                    f.Add<Reached>(entity);
            }
        }
    }
}