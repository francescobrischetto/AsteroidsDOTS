using Components.Data;
using Components.Tags;
using System;
using Unity.Entities;
using Unity.Jobs;

namespace Systems
{
    [UpdateAfter(typeof(SplitAsteroidsSystem))]
    public class DestroyableSystem : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem _endSimulationECBS;
        public event EventHandler OnShipDestroyed;
        public struct OnShipDestroyedEvent : IComponentData { public int Value; }

        private DOTSEvents_NextFrame<OnShipDestroyedEvent> dotsEvents;

        protected override void OnCreate()
        {
            base.OnCreate();
            _endSimulationECBS = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
            dotsEvents = new DOTSEvents_NextFrame<OnShipDestroyedEvent>(World);
        }
        protected override void OnUpdate()
        {
            var ecb = _endSimulationECBS.CreateCommandBuffer().AsParallelWriter();
            DOTSEvents_NextFrame<OnShipDestroyedEvent>.EventTrigger eventTrigger = dotsEvents.GetEventTrigger();
            var playerComponents = GetComponentDataFromEntity<PlayerTagComponent>();
            JobHandle jobHandle = Entities.ForEach((Entity entity, int entityInQueryIndex, in DestroyableDataComponent destroyableData) =>
            {
                if(destroyableData.ShouldBeDestroyed)
                {
                    //Specific condition
                    if (playerComponents.HasComponent(entity))
                    {
                        eventTrigger.TriggerEvent(entityInQueryIndex);
                        
                    }
                    ecb.DestroyEntity(entityInQueryIndex, entity);     
                }

            }).WithReadOnly(playerComponents).ScheduleParallel(Dependency);
            dotsEvents.CaptureEvents(eventTrigger, jobHandle, (OnShipDestroyedEvent onShipDestroyedEvent) => {
                OnShipDestroyed?.Invoke(this, EventArgs.Empty);
            });
            Dependency = JobHandle.CombineDependencies(jobHandle, Dependency);
            _endSimulationECBS.AddJobHandleForProducer(Dependency);
        }
    }
}
