using Components.Data;
using Components.Tags;
using Monobehaviours;
using System;
using Unity.Entities;
using Unity.Jobs;
using Utils;

namespace Systems
{
    [UpdateAfter(typeof(SplitAsteroidsSystem))]
    public class DestroyableSystem : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem _endSimulationECBS;
        public event EventHandler OnShipDestroyed;
        public event EventHandler<ObjectPoints> OnObjectDestroyed;
        public struct OnShipDestroyedEvent : IComponentData { public int Value; }
        public struct OnObjectDestroyedEvent : IComponentData { public ObjectPoints DestroyedObject; }

        private DOTSEvents_NextFrame<OnShipDestroyedEvent> dotsEventShip;
        private DOTSEvents_NextFrame<OnObjectDestroyedEvent> dotsEventObjects;

        protected override void OnCreate()
        {
            base.OnCreate();
            _endSimulationECBS = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
            dotsEventShip = new DOTSEvents_NextFrame<OnShipDestroyedEvent>(World);
            dotsEventObjects = new DOTSEvents_NextFrame<OnObjectDestroyedEvent>(World);
        }
        protected override void OnUpdate()
        {
            var ecb = _endSimulationECBS.CreateCommandBuffer().AsParallelWriter();
            DOTSEvents_NextFrame<OnShipDestroyedEvent>.EventTrigger eventTriggerShip = dotsEventShip.GetEventTrigger();
            DOTSEvents_NextFrame<OnObjectDestroyedEvent>.EventTrigger eventTriggerObjects = dotsEventObjects.GetEventTrigger();
            var playerComponents = GetComponentDataFromEntity<PlayerTagComponent>();
            var ufoComponents = GetComponentDataFromEntity<UfoTagComponent>();
            var asteroidComponents = GetComponentDataFromEntity<AsteroidTagComponent>();
            JobHandle jobHandle = Entities.ForEach((Entity entity, int entityInQueryIndex, in DestroyableDataComponent destroyableData) =>
            {
                if(destroyableData.ShouldBeDestroyed)
                {
                    if (playerComponents.HasComponent(entity))
                    {
                        eventTriggerShip.TriggerEvent(entityInQueryIndex);

                    }
                    else if(ufoComponents.HasComponent(entity))
                    {
                        ObjectPoints destroyedObject = ObjectPoints.LargeUfo;
                        switch (ufoComponents[entity].Type)
                        {
                            case UfoType.Large:
                                destroyedObject = ObjectPoints.LargeUfo;
                                break;
                            case UfoType.Small:
                                destroyedObject = ObjectPoints.SmallUfo;
                                break;

                        }
                        eventTriggerObjects.TriggerEvent(entityInQueryIndex, new OnObjectDestroyedEvent
                        {
                            DestroyedObject = destroyedObject
                        });
                    }
                    else if (asteroidComponents.HasComponent(entity))
                    {
                        ObjectPoints destroyedObject = ObjectPoints.LargeAsteroid;
                        switch (asteroidComponents[entity].Type)
                        {
                            case AsteroidType.Large:
                                destroyedObject = ObjectPoints.LargeAsteroid;
                                break;
                            case AsteroidType.Medium:
                                destroyedObject = ObjectPoints.MediumAsteroid;
                                break;
                            case AsteroidType.Small:
                                destroyedObject = ObjectPoints.SmallAsteroid;
                                break;

                        }
                        eventTriggerObjects.TriggerEvent(entityInQueryIndex, new OnObjectDestroyedEvent
                        {
                            DestroyedObject = destroyedObject
                        });
                    }
                    ecb.DestroyEntity(entityInQueryIndex, entity);     
                }

            }).WithReadOnly(playerComponents).WithReadOnly(ufoComponents).WithReadOnly(asteroidComponents).ScheduleParallel(Dependency);
            dotsEventShip.CaptureEvents(eventTriggerShip, jobHandle, (OnShipDestroyedEvent onShipDestroyedEvent) => {
                OnShipDestroyed?.Invoke(this, EventArgs.Empty);
            });
            dotsEventObjects.CaptureEvents(eventTriggerObjects, jobHandle, (OnObjectDestroyedEvent onObjectDestroyedEvent) => {
                OnObjectDestroyed?.Invoke(this, onObjectDestroyedEvent.DestroyedObject);
            });
            Dependency = JobHandle.CombineDependencies(jobHandle, Dependency);
            _endSimulationECBS.AddJobHandleForProducer(Dependency);
        }
    }
}
