using Quantum.QuantumUser.Simulation.Gameplay.Features.AttackTargets.Systems;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.AttackTargets
{
    public class AttackTargetsFeature : SystemGroup
    {
        public AttackTargetsFeature() : base(nameof(AttackTargetsFeature), CreateSystems())
        {
        }

        private static SystemBase[] CreateSystems()
        {
            return new SystemBase[]
            {
                new CurrentTargetSetSystem()
            };
        }
    }
}