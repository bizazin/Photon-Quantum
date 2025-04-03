using Photon.Deterministic;
using Quantum.QuantumUser.Simulation.Common.Extensions;
using Quantum.QuantumUser.Simulation.Gameplay.Features.Cooldowns;
using UnityEngine.Scripting;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Player.Systems
{
   [Preserve]
   public unsafe class PlayerStatesHandleSystem : SystemMainThreadFilter<PlayerStatesHandleSystem.Filter>
   {
      public override void Update(Frame f, ref Filter filter)
      {
         Direction direction = f.Get<Direction>(filter.Entity);
         var hasTarget = f.Has<CurrentTarget>(filter.Entity);
         var isAttacking = f.Get<PlayerActionState>(filter.Entity).Value == EPlayerActionState.Attacking;

         if (hasTarget && direction.Value == FPVector3.Zero)
         {
            if (!isAttacking)
               HandleAttackPreparing(f, ref filter);
            else
               f.Set(filter.Entity, new PlayerActionState { Value = EPlayerActionState.Attacking });
         }
         else if (direction.Value != FPVector3.Zero)
         {
            if (f.Has<AttackPreparingDelay>(filter.Entity))
               f.Remove<AttackPreparingDelay>(filter.Entity);

            f.Set(filter.Entity, new PlayerActionState { Value = EPlayerActionState.Moving });
         }
         else
         {
            if (f.Has<AttackPreparingDelay>(filter.Entity))
               f.Remove<AttackPreparingDelay>(filter.Entity);

            f.Set(filter.Entity, new PlayerActionState { Value = EPlayerActionState.Idle });
         }
      }

      private void HandleAttackPreparing(Frame f, ref Filter filter)
      {
         if (!f.Has<AttackPreparingDelay>(filter.Entity))
         {
            var baseStats = f.ResolveDictionary(filter.BaseStats->Value);
            f.Set(filter.Entity, new AttackPreparingDelay { Value = baseStats[EStats.AttackDelay] });
            f.Set(filter.Entity, new PlayerActionState { Value = EPlayerActionState.AttackPreparing });
         }
         else
         {
            AttackPreparingDelay* attackPreparing = f.Unsafe.GetPointer<AttackPreparingDelay>(filter.Entity);
            attackPreparing->Value -= f.DeltaTime;

            if (attackPreparing->Value <= FP._0)
            {
               f.Remove<AttackPreparingDelay>(filter.Entity);
               f.Set(filter.Entity, new PlayerActionState { Value = EPlayerActionState.Attacking });
            }
         }
      }

      public struct Filter
      {
         public EntityRef Entity;
         public PlayerLink* Link;
         public Owner* Owner;
         public BaseStats* BaseStats;
         public PlayerActionState* PlayerState;
      }
   }
}
