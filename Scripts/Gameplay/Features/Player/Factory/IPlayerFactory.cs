using System.Collections.Generic;
using Photon.Deterministic;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Player.Factory
{
    public interface IPlayerFactory
    {
        public EntityRef CreatePlayer(Frame f, PlayerRef player, FPVector3 at, int teamIndex, bool isDummy = false);
    }
}