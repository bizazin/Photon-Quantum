using Photon.Deterministic;
using Quantum.QuantumUser.Simulation.Common.Di;
using Quantum.QuantumUser.Simulation.Gameplay.Features.Armaments.Factory;
using Quantum.QuantumUser.Simulation.Gameplay.Features.Cooldowns;
using Quantum.QuantumUser.Simulation.Gameplay.Features.Weapons.Configs;
using Quantum.QuantumUser.Simulation.Gameplay.StaticData;
using UnityEditor;
using UnityEngine;
using UnityEngine.Scripting;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Weapons.Systems
{
    [Preserve]
    public unsafe class OrbitingShotWeaponSystem : SystemMainThread
    {
        private IArmamentFactory _armamentFactory;
        private IStaticDataService _staticDataService;

        public override void OnInit(Frame f)
        {
            _armamentFactory = DI.Resolve<IArmamentFactory>();
            _staticDataService = DI.Resolve<IStaticDataService>();
        }

        public override void Update(Frame f)
        {
            var weapons = f.Filter<OrbitalShotWeapon, CooldownUp, Owner>();
            var heroes = f.Filter<PlayerLink, PlayerActionState, Transform3D>();

            while (heroes.NextUnsafe(
                       out EntityRef heroEntity,
                       out PlayerLink* heroLink,
                       out PlayerActionState* actionState,
                       out Transform3D* heroPosition))
            {
                EntityRef weaponEntity = FindWeaponForHero(weapons, heroEntity);

                if (weaponEntity == EntityRef.None)
                    continue;

                ExecuteAttack(f, heroLink, heroPosition, weaponEntity);
            }
        }

        private EntityRef FindWeaponForHero(ComponentFilter<OrbitalShotWeapon, CooldownUp, Owner> abilities,
            EntityRef heroEntity)
        {
            while (abilities.NextUnsafe(out EntityRef weaponRef, out _, out _, out Owner* owner))
                if (owner->Link.Entity == heroEntity)
                    return weaponRef;

            return EntityRef.None;
        }

        private void ExecuteAttack(Frame f, PlayerLink* heroLink, Transform3D* heroPosition, EntityRef weaponEntity)
        {
            int level = 1;
            WeaponLevel weaponLevel = _staticDataService.GetWeaponLevel(EWeaponId.OrbitalShot, level);

            int projectileCount = weaponLevel.ProjectileSetup.ProjectileCount;

            for (int i = 0; i < projectileCount; i++)
            {
                FP phase = FP._2 * FP.Pi * i / projectileCount;

                CreateProjectile(f, heroLink->Entity, heroPosition->Position, phase, level, weaponEntity);
            }

            weaponEntity.PutOnCooldown(f, _staticDataService.GetWeaponLevel(EWeaponId.OrbitalShot, level).Cooldown);

            f.Events.PlayerAttack(heroLink->Entity);
        }

        private void CreateProjectile(Frame f, EntityRef heroEntity, FPVector3 heroPos, FP phase, int level, EntityRef weaponEntity)
        {
            EntityRef ballShotEntity = _armamentFactory.CreateBallShot(f, level, heroPos + FPVector3.Up, phase, f.Get<Owner>(heroEntity), weaponEntity);
            f.Set(ballShotEntity, new ProducerId { Value = heroEntity });
            f.Set(ballShotEntity, new OrbitCenterPosition { Value = heroPos });
            f.Set(ballShotEntity, new OrbitCenterFollowTarget { Value = heroEntity });

            f.Add<Moving>(ballShotEntity);
        }
    }
}