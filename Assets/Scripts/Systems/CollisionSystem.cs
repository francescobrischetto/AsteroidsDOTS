using Components.Data;
using Components.Stats;
using Components.Tags;
using System;
using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;
using Utils;

namespace Systems
{
    [UpdateAfter(typeof(LifetimeSystem))]
    public class CollisionSystem : JobComponentSystem
    {
        private BuildPhysicsWorld physicsWorld;
        private StepPhysicsWorld stepPhysicsWorld;
        private EndSimulationEntityCommandBufferSystem _endSimulationECBS;
        private EntityManager _entityManager;
        private EntityArchetype powerUpArchetype;
        public event EventHandler OnPowerUpPicked;
        public struct OnPowerUpPickedEvent : IComponentData { public int Value; }

        private DOTSEvents_NextFrame<OnPowerUpPickedEvent> dotsEvents;
        protected override void OnCreate()
        {
            base.OnCreate();
            physicsWorld = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<BuildPhysicsWorld>();
            stepPhysicsWorld = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<StepPhysicsWorld>();
            _endSimulationECBS = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
            _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            powerUpArchetype = _entityManager.CreateArchetype(
                typeof(PowerUpDataComponent),
                typeof(PowerUpStatsComponent)
            );
            dotsEvents = new DOTSEvents_NextFrame<OnPowerUpPickedEvent>(World);
        }

        [BurstCompile]
        private struct CollisionJob : ITriggerEventsJob
        {
            public ComponentDataFromEntity<DestroyableDataComponent> destroyables;
            public ComponentDataFromEntity<PlayerTagComponent> players;
            public ComponentDataFromEntity<PowerUpTagComponent> pickablePowerups;
            public ComponentDataFromEntity<PowerUpStatsComponent> powerupsStats;
            public EntityCommandBuffer.ParallelWriter entityCommandBuffer;
            public EntityArchetype powerUpArchetype;
            public DOTSEvents_NextFrame<OnPowerUpPickedEvent>.EventTrigger eventTrigger;

            public void Execute(TriggerEvent triggerEvent)
            {
                bool isPickablePowerUpCollision = IsCollidingWithPickablePowerUp(triggerEvent);

                if (isPickablePowerUpCollision)
                {
                    ManagePlayerPowerUpInteraction(triggerEvent);
                }
                else
                {
                    ManageRegularDestroyableInteraction(triggerEvent);
                }
            }

            private bool IsCollidingWithPickablePowerUp(TriggerEvent triggerEvent)
            {
                if (players.HasComponent(triggerEvent.EntityA) && pickablePowerups.HasComponent(triggerEvent.EntityB))
                {
                    return true;
                }
                if (players.HasComponent(triggerEvent.EntityB) && pickablePowerups.HasComponent(triggerEvent.EntityA))
                {
                    return true;
                }
                return false;
            }


            private void ManagePlayerPowerUpInteraction(TriggerEvent triggerEvent)
            {
                Entity pickablePowerUp = Entity.Null;
                Entity player = Entity.Null;
                int playerIndex = 0;
                if (players.HasComponent(triggerEvent.EntityA) && pickablePowerups.HasComponent(triggerEvent.EntityB))
                {
                    player = triggerEvent.EntityA;
                    playerIndex = triggerEvent.BodyIndexA;
                    pickablePowerUp = triggerEvent.EntityB;
                }
                else if (players.HasComponent(triggerEvent.EntityB) && pickablePowerups.HasComponent(triggerEvent.EntityA))
                {
                    player = triggerEvent.EntityB;
                    playerIndex = triggerEvent.BodyIndexB;
                    pickablePowerUp = triggerEvent.EntityA;
                }
                
                //Destroy Powerup Entity
                var entityToDestroy = destroyables[pickablePowerUp];
                entityToDestroy.ShouldBeDestroyed = true;
                destroyables[pickablePowerUp] = entityToDestroy;
                
                //Create entity powerup
                Entity powerUpEntity = entityCommandBuffer.CreateEntity(playerIndex,powerUpArchetype);
                entityCommandBuffer.SetComponent(playerIndex, powerUpEntity, new PowerUpDataComponent
                {
                    MaxTime = powerupsStats[pickablePowerUp].GrantedTime,
                    ElapsedTime = 0,
                    TargetEntity = player
                });
                entityCommandBuffer.SetComponent(playerIndex, powerUpEntity, new PowerUpStatsComponent
                {
                    Type = powerupsStats[pickablePowerUp].Type
                });
                eventTrigger.TriggerEvent(playerIndex);
            }

            private void ManageRegularDestroyableInteraction(TriggerEvent triggerEvent)
            {
                if(destroyables.HasComponent(triggerEvent.EntityA) && destroyables.HasComponent(triggerEvent.EntityB))
                {
                    var entityA = destroyables[triggerEvent.EntityA];
                    var entityB = destroyables[triggerEvent.EntityB];

                    if (entityA.IsInvulnerable || entityB.IsInvulnerable)
                    {
                        return;
                    }

                    entityA.ShouldBeDestroyed = true;
                    destroyables[triggerEvent.EntityA] = entityA;
                
                    entityB.ShouldBeDestroyed = true;
                    destroyables[triggerEvent.EntityB] = entityB;
                }
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var destroyablesComponents = GetComponentDataFromEntity<DestroyableDataComponent>();
            var playersComponents = GetComponentDataFromEntity<PlayerTagComponent>();
            var powerupsComponents = GetComponentDataFromEntity<PowerUpTagComponent>();
            var powerupsStatsComponents = GetComponentDataFromEntity<PowerUpStatsComponent>();
            var ecb = _endSimulationECBS.CreateCommandBuffer().AsParallelWriter();
            DOTSEvents_NextFrame<OnPowerUpPickedEvent>.EventTrigger eventTrigger = dotsEvents.GetEventTrigger();

            var job = new CollisionJob
            {
                destroyables = destroyablesComponents,
                players = playersComponents,
                pickablePowerups = powerupsComponents,
                powerupsStats = powerupsStatsComponents,
                entityCommandBuffer = ecb,
                powerUpArchetype = powerUpArchetype,
                eventTrigger = eventTrigger
            };
            var jobHandle = job.Schedule(stepPhysicsWorld.Simulation, ref physicsWorld.PhysicsWorld, inputDeps);
            dotsEvents.CaptureEvents(eventTrigger, jobHandle, (OnPowerUpPickedEvent onShipDestroyedEvent) => {
                OnPowerUpPicked?.Invoke(this, EventArgs.Empty);
            });
            inputDeps = JobHandle.CombineDependencies(jobHandle, inputDeps);
            _endSimulationECBS.AddJobHandleForProducer(inputDeps);
            return jobHandle;
        }

    }
}