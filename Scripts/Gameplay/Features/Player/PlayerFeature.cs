using Quantum.QuantumUser.Simulation.Gameplay.Common.Time;
using Quantum.QuantumUser.Simulation.Gameplay.Features.Movement;
using Quantum.QuantumUser.Simulation.Gameplay.Features.Movement.Systems;
using Quantum.QuantumUser.Simulation.Gameplay.Features.Player.Systems;
using UnityEngine.Scripting;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Player
{

    [Preserve]
    public unsafe class PlayerFeature : SystemGroup
    {
        public PlayerFeature() : base(nameof(PlayerFeature), CreateSystems())
        {
        }

        private static SystemBase[] CreateSystems()
        {
            return new SystemBase[]
            {
                new PlayerCreationSystem(),
                new PlayerDirectionSetSystem(),
                new PlayerSetLookRotationByAttackSystem(),
                new PlayerStatesHandleSystem(),
                new PlayerMovementSystem(),
                new PlayerDeathSystem(),
                new AnimationStatesEventsSystem(),
                new FinalizeHeroDeathProcessingSystem(),
                new TimeSystem(),
            };
        }
    }
    
}
