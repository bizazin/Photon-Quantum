using Photon.Deterministic;
using Quantum.QuantumUser.Simulation.Common.Di;
using Quantum.QuantumUser.Simulation.Gameplay.Features.Armaments.Factory;
using Quantum.QuantumUser.Simulation.Gameplay.Features.Cooldowns;
using Quantum.QuantumUser.Simulation.Gameplay.Features.Weapons.Configs;
using Quantum.QuantumUser.Simulation.Gameplay.StaticData;
using UnityEngine;
using UnityEngine.Scripting;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Weapons.Systems
{
    [Preserve]
    public unsafe class BasicShotWeaponSystem : SystemMainThread
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
            ComponentFilter<BowShotWeapon, CooldownUp, Owner> weapons = f.Filter<BowShotWeapon, CooldownUp, Owner>();
            ComponentFilter<PlayerLink, PlayerActionState, CurrentTarget, Transform3D> heroes = f.Filter<PlayerLink, PlayerActionState, CurrentTarget, Transform3D>();

            while (heroes.NextUnsafe(
                out EntityRef heroEntity,
                out PlayerLink* heroLink,
                out PlayerActionState* actionState,
                out CurrentTarget* currentTarget,
                out Transform3D* heroPosition))
            {
                if (actionState->Value != EPlayerActionState.Attacking)
                    continue;

                EntityRef weaponEntity = FindWeaponForHero(weapons, heroEntity);
                if (weaponEntity == EntityRef.None)
                    continue;

                if (!TryGetAttackDirection(f, currentTarget, heroPosition, out FPVector3 attackDirection))
                    continue;

                ExecuteAttack(f, heroEntity, heroLink, heroPosition, attackDirection, weaponEntity);
            }
        }

        private EntityRef FindWeaponForHero(ComponentFilter<BowShotWeapon, CooldownUp, Owner> abilities, EntityRef heroEntity)
        {
            while (abilities.NextUnsafe(out EntityRef weaponRef, out _, out _, out Owner* owner))
                if (owner->Link.Entity == heroEntity)
                    return weaponRef;
            
            return EntityRef.None;
        }

        private bool TryGetAttackDirection(Frame f, CurrentTarget* currentTarget, Transform3D* heroPosition, out FPVector3 attackDirection)
        {
            attackDirection = FPVector3.Zero;

            EntityRef targetEntity = currentTarget->Value.Entity;
            if (!f.Has<Transform3D>(targetEntity))
                return false;

            Transform3D targetPosition = f.Get<Transform3D>(targetEntity);
            FPVector3 direction = targetPosition.Position - heroPosition->Position;
            attackDirection = new FPVector3(direction.X, 0, direction.Z).Normalized;

            return true;
        }
        
        private void ExecuteAttack(Frame f, EntityRef heroEntity, PlayerLink* heroLink, Transform3D* heroPosition,
            FPVector3 attackDirection, EntityRef weaponEntity)
        {
            Debug.Log($"SHOT from {heroEntity}");

            WeaponLevel weaponLevel = _staticDataService.GetWeaponLevel(EWeaponId.BowShot, 1);

            _armamentFactory.CreatePendingShots(f, EWeaponId.BowShot, weaponEntity,
                f.Get<Owner>(heroLink->Entity), attackDirection);

            weaponEntity.PutOnCooldown(f, _staticDataService.GetWeaponLevel(EWeaponId.BowShot, 1).Cooldown);

            f.Events.PlayerAttack(heroLink->Entity);
        }
    }
}
