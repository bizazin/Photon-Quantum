namespace Quantum.QuantumUser.Simulation.Gameplay.Features.TargetsCollection
{
    public static class TargetCollectionEntityExtensions
    {
        public static void RemoveTargetCollectionComponents(this EntityRef entity, Frame f)
        {
            if (f.Has<TargetBuffer>(entity))
                f.Remove<TargetBuffer>(entity);
            
            if (f.Has<CollectTargetsInterval>(entity))
                f.Remove<CollectTargetsInterval>(entity);
            
            if (f.Has<CollectTargetsTimer>(entity))
                f.Remove<CollectTargetsTimer>(entity);

            f.Remove<ReadyToCollectTargets>(entity);
        }
    }
}