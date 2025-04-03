using System.Collections.Generic;
using Photon.Deterministic;
using Quantum.Prototypes;
using Quantum.QuantumUser.Simulation.Common.Di;
using Quantum.QuantumUser.Simulation.Gameplay.Common.CardsSetup;
using Quantum.QuantumUser.Simulation.Gameplay.Features.Player.Factory;
using Quantum.QuantumUser.Simulation.Gameplay.Features.Weapons.Factory;
using Quantum.QuantumUser.Simulation.Gameplay.Levels;
using Quantum.QuantumUser.Simulation.Gameplay.StaticData;
using UnityEngine;
using UnityEngine.Scripting;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Player.Systems
{
    [Preserve]
    public class PlayerCreationSystem : SystemSignalsOnly, ISignalOnPlayerAdded
    {
        private IStaticDataService _staticDataService;
        private IPlayerFactory _playerFactory;
        private IWeaponFactory _weaponFactory;
        private IDeckUtility _deckUtility;
        private readonly bool _createDummy = true;

        public override void OnInit(Frame f)
        {
            _playerFactory = DI.Resolve<IPlayerFactory>();
            _weaponFactory = DI.Resolve<IWeaponFactory>();
            _deckUtility = DI.Resolve<IDeckUtility>();
            
            _deckUtility.Init();
        }

        public void OnPlayerAdded(Frame f, PlayerRef player, bool firstTime)
        {
            var teamIndex = GetTeamIndex(player);
            
            FPVector3 spawnPosition = GetSpawnPositionForTeam(f, teamIndex);
            
            EntityRef playerEntity = _playerFactory.CreatePlayer(f, player, spawnPosition, teamIndex);
            
            _deckUtility.AddCardAbilityToOrbitalShotWeapon(f, CreateOrbitingShotWeapon(f, playerEntity), EAbilityCardId.FireBall);
            
            _deckUtility.AddCardAbilityToOrbitalShotWeapon(f, CreateOrbitingShotWeapon(f, playerEntity), EAbilityCardId.FrostBall);
            
            if (teamIndex == 1)
            {
                EntityRef basicShotAbility = _weaponFactory.CreateBowShotWeapon(f, level: 1, f.Get<Owner>(playerEntity));
                _deckUtility.AddCardAbilityToBowShotWeapon(f, basicShotAbility, EAbilityCardId.FireArrow);
                // _deckUtility.AddCardAbilityToBowShotWeapon(f, basicShotAbility, EAbilityCardId.FrostArrow);
            }
            else
            {
                EntityRef basicShotAbility = _weaponFactory.CreateBowShotWeapon(f, level: 1, f.Get<Owner>(playerEntity));
                // _deckUtility.AddCardAbilityToBowShotWeapon(f, basicShotAbility, EAbilityCardId.FireArrow);
                _deckUtility.AddCardAbilityToBowShotWeapon(f, basicShotAbility, EAbilityCardId.FrostArrow);
            }

            if (_createDummy)
            {
                EntityRef dummy = _playerFactory.CreatePlayer(f, player, new FPVector3(0,0,0), teamIndex == 0 ? 1 : 0, true);
            }
        }

        private FPVector3 GetSpawnPositionForTeam(Frame f, int teamIndex)
        {
            ComponentPrototypeSet[] mapEntities = f.Map.MapEntities;

            if (teamIndex < mapEntities.Length)
            {
                foreach (var component in mapEntities[teamIndex].Components)
                {
                    if (component is Transform3DPrototype transformPrototype)
                    {
                        Debug.Log($"Spawn Position for Team {teamIndex}: {transformPrototype.Position}");
                        return transformPrototype.Position;
                    }
                }
            }

            Debug.LogError($"No Transform3DPrototype found for team index {teamIndex}");
            return FPVector3.Zero;
        }


        private int GetTeamIndex(PlayerRef player) => 
            player % 2 == 1 ? 0 : 1;

        private EntityRef CreateOrbitingShotWeapon(Frame f, EntityRef playerEntity) => 
            _weaponFactory.CreateOrbitingShotWeapon(f, level: 1, f.Get<Owner>(playerEntity));
    }
}