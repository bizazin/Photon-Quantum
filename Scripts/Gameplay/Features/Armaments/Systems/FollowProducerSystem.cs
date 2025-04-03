namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Armaments.Systems
{
    public unsafe class FollowProducerSystem : SystemMainThread
    {
        public override void Update(Frame f)
        {
            var followers = f.Filter<
                FollowingProducer,
                WorldPosition,
                ProducerId>();
            
            var producers = f.Filter<WorldPosition>();

            while (followers.NextUnsafe(
                       out EntityRef follower,
                       out _,
                       out _,
                       out ProducerId* producerId))
            while (producers.NextUnsafe(
                       out EntityRef producer,
                       out _))
            {
                if (producerId->Value == producer) 
                    f.Unsafe.GetPointer<WorldPosition>(follower)->Value = f.Unsafe.GetPointer<WorldPosition>(producer)->Value;
            }
        }
    }
}