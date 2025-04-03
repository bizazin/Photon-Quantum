using UnityEngine.Scripting;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Statuses.Systems
{
    [Preserve]
    public unsafe class ApplyFreezeStatusSystem : SystemMainThread
    {
        public override void Update(Frame f)
        {
            var without = ComponentSet.Create<Affected>();
            var statuses = f.Filter<
                Status,
                Freeze,
                ProducerId,
                TargetId,
                EffectValue>
                (without);
            
            while (statuses.NextUnsafe(
                       out EntityRef statusRef,
                       out Status* status,
                       out Freeze* freeze,
                       out ProducerId* producerId,
                       out TargetId* targetId,
                       out EffectValue* effectValue
                       ))

            {
              
                    
                EntityRef statusEntity = f.Create();
                f.Set(statusEntity, new StatChange{Value = EStats.Speed});
                f.Set(statusEntity, new TargetId {Value = targetId->Value});
                f.Set(statusEntity, new ProducerId{Value = producerId->Value});
                f.Set(statusEntity, new EffectValue{Value = effectValue->Value});
                f.Set(statusEntity, new ApplierStatusLink{Value = statusEntity.Index});
                
                f.Set(statusRef, new Affected());
            }  
        }
    }
}