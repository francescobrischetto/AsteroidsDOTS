using Components.Data;
using Unity.Entities;

namespace Systems
{
    [UpdateAfter(typeof(SplitAsteroidsSystem))]
    public class DestroyableSystem : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem _endSimulationECBS;

        protected override void OnCreate()
        {
            base.OnCreate();
            _endSimulationECBS = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }
        protected override void OnUpdate()
        {
            var ecb = _endSimulationECBS.CreateCommandBuffer().AsParallelWriter();
            Entities.ForEach((Entity entity, int entityInQueryIndex, in DestroyableDataComponent destroyableData) =>
            {
                if(destroyableData.ShouldBeDestroyed)
                {
                    ecb.DestroyEntity(entityInQueryIndex, entity);
                }

            }).ScheduleParallel();

            _endSimulationECBS.AddJobHandleForProducer(Dependency);
        }
    }
}
