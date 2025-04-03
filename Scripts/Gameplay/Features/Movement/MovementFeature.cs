using Quantum.QuantumUser.Simulation.Gameplay.Features.Movement.Systems;
using UnityEngine.Scripting;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Movement
{

    [Preserve]
    public unsafe class MovementFeature : SystemGroup
    {
        public MovementFeature() : base(nameof(MovementFeature), CreateSystems())
        {
        }

        private static SystemBase[] CreateSystems()
        {
            return new SystemBase[]
            {
                new OrbitalDeltaMoveSystem(),
                new OrbitCenterFollowSystem(),
               
                new SetRotationByDirectionSystem(),
                new SetRotationByDirectionLerpSystem(),
                new SetRotationByLookDirectionSystem(),
            	new UpdateTransformPositionSystem(),
                new DirectionDeltaMovingSystem(),
                // new DirectionDeltaMovingPlayerSystem(),
            };
        }
    }
}
