using System;
using Photon.Deterministic;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Weapons.Configs
{
    [Serializable]
    public class ProjectileSetup
    {
        public int ProjectileCount = 1;

        public int PendingShotsCount = 1;
        public FP PendingShotInterval = FP._0_20;

        public FP Speed;
        public int Pierce = 1;
        public FP ContactRadius;
        public FP Lifetime;
        public EOrbitLevel OrbitLevel;
		public FP MuzzleDistance = 1; // distance from player center for creating projectiles
    }
}