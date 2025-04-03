using System.Collections.Generic;
using Photon.Deterministic;
using Quantum.Collections;
using Quantum.QuantumUser.Simulation.Common.Extensions;
using Quantum.QuantumUser.Simulation.Gameplay.Features.CharacterStats;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Player.Factory
{
    public unsafe class PlayerFactory : IPlayerFactory
    {
        public EntityRef CreatePlayer(Frame f, PlayerRef player, FPVector3 at, int teamIndex, bool isDummy = false)
        {
            Dictionary<EStats, FP> baseStats = InitStats.EmptyStatDictionary()
                .With(x => x[EStats.Speed] = isDummy ? 0 : 4)
                .With(x => x[EStats.MaxHp] = isDummy ? 1000 : 100)
                .With(x => x[EStats.AttackDelay] = FP._0_50)
                ;
            
            RuntimePlayer runtimePlayer = f.GetPlayerData(player);
            
            EntityRef heroEntity = f.Create(runtimePlayer.PlayerAvatar);
            PlayerLink playerLink = new PlayerLink
            {
                Value = player,
                Entity = heroEntity
            };
            
            f.Set(heroEntity, playerLink);
            f.Set(heroEntity, new Owner
            {
                Link = playerLink,
                TeamIndex = teamIndex
            });
            
            f.Unsafe.GetPointer<Transform3D>(heroEntity)->Position = at;

            QDictionaryPtr<EStats, FP> baseStatsDict = f.AllocateDictionary<EStats, FP>();
            f.Set(heroEntity, new BaseStats { Value = baseStatsDict });
            f.FillEnumDictionaryComponent(baseStatsDict, baseStats);

            QDictionaryPtr<EStats, FP> statsModifiersDict = f.AllocateDictionary<EStats, FP>();
            f.Set(heroEntity, new StatsModifiers { Value = statsModifiersDict });
            f.FillEnumDictionaryComponent(statsModifiersDict, InitStats.EmptyStatDictionary());
            
            f.Set(heroEntity, new Direction { Value = FPVector3.Zero });
            f.Set(heroEntity, new LookDirection { Value = FPVector3.Zero });
            f.Set(heroEntity, new Speed { Value = baseStats[EStats.Speed] });
            f.Set(heroEntity, new RotationSpeed { Value = 5 });
            f.Set(heroEntity, new CurrentHp { Value = baseStats[EStats.MaxHp] });
            f.Set(heroEntity, new MaxHp { Value = baseStats[EStats.MaxHp] });
            
            f.Set(heroEntity, new PlayerActionState { Value = EPlayerActionState.None });
            f.Set(heroEntity, new PlayerLifeState { Value = EPlayerLifeState.Alive });
            
            if(!isDummy)
                f.Add<MovementAvailable>(heroEntity);

            return heroEntity;
        }
    }
}