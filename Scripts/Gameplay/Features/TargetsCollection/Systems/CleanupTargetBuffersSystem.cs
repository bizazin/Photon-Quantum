using Quantum.QuantumUser.Simulation.Gameplay.Features.Cleanup;
using Quantum.Task;
using UnityEngine.Scripting;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.TargetsCollection.Systems
{
    [Preserve]
    public unsafe class CleanupTargetBuffersSystem : SystemBase, ICleanupSystem
    {
        protected override TaskHandle Schedule(Frame f, TaskHandle taskHandle) => 
            taskHandle;

        public void Cleanup(Frame f)
        {
            ComponentFilter<TargetBuffer> entities = f.Filter<TargetBuffer>();

            while (entities.NextUnsafe(
                       out _,
                       out TargetBuffer* targetsBuffer)) 
                f.ResolveList(targetsBuffer->Value).Clear();
        }
    }
}