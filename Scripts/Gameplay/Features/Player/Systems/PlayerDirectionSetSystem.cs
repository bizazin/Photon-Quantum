using Photon.Deterministic;
using UnityEngine.Scripting;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Player.Systems
{
    [Preserve]
    public unsafe class PlayerDirectionSetSystem: SystemMainThreadFilter<PlayerDirectionSetSystem.Filter>
    {
        public override void Update(Frame f, ref Filter filter)
        {
            if (filter.PlayerLifeState->Value == EPlayerLifeState.Dead)
            {
                f.Unsafe.GetPointer<Direction>(filter.Entity)->Value = FPVector3.Zero;
                return;
            }
            
            Input* input = f.GetPlayerInput(filter.Link->Value);
            
            FPVector3 direction = FPVector3.Zero;
            
            if (input->UpButton)
                direction += FPVector3.Forward;
            if (input->DownButton)
                direction += FPVector3.Back;
            if (input->LeftButton)
                direction += FPVector3.Left;
            if (input->RightButton)
                direction += FPVector3.Right;
            
            direction += input->Direction;

            if (direction != FPVector3.Zero)
                direction = direction.Normalized;
            
            f.Unsafe.GetPointer<Direction>(filter.Entity)->Value = direction;

        }

        public struct Filter
        {
            public EntityRef Entity;
            public PlayerLink* Link;
            public PlayerLifeState* PlayerLifeState;
        }
    }
}