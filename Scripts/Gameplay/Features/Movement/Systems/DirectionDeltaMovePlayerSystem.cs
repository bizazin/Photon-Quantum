using UnityEngine.Scripting;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Movement.Systems
{
	namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Movement.Systems
	{
		[Preserve]
		public unsafe class DirectionDeltaMovingPlayerSystem : SystemMainThreadFilter<DirectionDeltaMovingPlayerSystem.Filter>
		{
			public override void Update(Frame f, ref Filter filter)
			{
				var baseStats = f.ResolveDictionary(filter.BaseStats->Value);

				var newPosition = filter.WorldPosition->Value + filter.Direction->Value 
					* baseStats[EStats.Speed] * f.DeltaTime;
            
				f.Set(filter.Entity, new WorldPosition { Value = newPosition });
			}
        
			public struct Filter
			{
				public EntityRef Entity;
				public WorldPosition* WorldPosition;
				public Direction* Direction;
				public BaseStats* BaseStats;
				public MovementAvailable* MovementAvailable;
				public Moving* Moving;
			}
		}
	}
}