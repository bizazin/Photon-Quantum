using System.Collections.Generic;
using Quantum.QuantumUser.Simulation.Common.Destruct;
using Quantum.QuantumUser.Simulation.Gameplay.Features.Armaments;
using Quantum.QuantumUser.Simulation.Gameplay.Features.AttackTargets;
using Quantum.QuantumUser.Simulation.Gameplay.Features.CharacterStats;
using Quantum.QuantumUser.Simulation.Gameplay.Features.Cleanup;
using Quantum.QuantumUser.Simulation.Gameplay.Features.EffectApplication;
using Quantum.QuantumUser.Simulation.Gameplay.Features.Effects;
using Quantum.QuantumUser.Simulation.Gameplay.Features.Lifetime;
using Quantum.QuantumUser.Simulation.Gameplay.Features.Movement;
using Quantum.QuantumUser.Simulation.Gameplay.Features.Player;
using Quantum.QuantumUser.Simulation.Gameplay.Features.Statuses.Quantum.QuantumUser.Simulation.Features.Movement;
using Quantum.QuantumUser.Simulation.Gameplay.Features.TargetsCollection;
using Quantum.QuantumUser.Simulation.Gameplay.Features.Weapons;

namespace Quantum
{
    public static partial class DeterministicSystemSetup
    {
        static partial void AddSystemsUser(ICollection<SystemBase> systems, RuntimeConfig gameConfig,
            SimulationConfig simulationConfig, SystemsConfig systemsConfig)
        {
            // The system collection is already filled with systems coming from the SystemsConfig. 
            // Add or remove systems to the collection: systems.Add(new SystemFoo());
            systems.Add(new PlayerFeature());
            systems.Add(new DeathFeature());
            
            systems.Add(new MovementFeature());
            systems.Add(new AttackTargetsFeature());
            systems.Add(new WeaponFeature());
            
            systems.Add(new ArmamentFeature());
            
            systems.Add(new TargetsCollectionFeature());
            systems.Add(new EffectApplicationFeature());
            
            systems.Add(new StatusFeature());
            systems.Add(new EffectFeature());
            systems.Add(new CharacterStatsFeature());
            
            systems.Add(new ProcessDestructedFeature());
            
            systems.Add(new CleanupFeature(systems));
        }
    }
}