using UnityEngine.Scripting;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Player.Systems
{
    [Preserve]
    public unsafe class HeroDeathSystem : SystemMainThreadFilter<HeroDeathSystem.Filter>
    {
        public override void Update(Frame f, ref Filter filter)
        {
            f.Remove<MovementAvailable>(filter.Hero);   
        }

        public struct Filter
        {
            public EntityRef Hero;
            public PlayerLink* PlayerLink;
            public Dead* Dead;
            public ProcessingDeath* ProcessingDeath;
        }
    }
}