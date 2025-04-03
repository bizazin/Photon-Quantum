using UnityEngine.Scripting;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Player.Systems
{
    [Preserve]
    public unsafe class PlayerDeathSystem : SystemMainThreadFilter<PlayerDeathSystem.Filter>
    {
        public override void Update(Frame f, ref Filter filter)
        {
            f.Set(filter.Entity, new PlayerLifeState { Value = EPlayerLifeState.Dead });

            
            //TODO  
            // add logic for enabling collider
            // f.Unsafe.GetPointer<PhysicsCollider3D>(filter.Entity)->IsTrigger = true;
            // f.Remove<PhysicsCollider3D>(filter.Entity);
            
            f.Remove<MovementAvailable>(filter.Entity);

            f.Events.PlayerDead(filter.Entity);
        }

        public struct Filter
        {
            public EntityRef Entity;
            public PlayerRef* PlayerRef;
            public Dead* Dead;
            public ProcessingDeath* ProcessingDeath;
            public PhysicsCollider3D* PhysicsCollider3D;
        }
    }

}