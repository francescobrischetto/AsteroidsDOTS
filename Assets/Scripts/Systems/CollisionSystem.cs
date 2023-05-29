using Components.Data;
using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;

namespace Systems
{
    [UpdateAfter(typeof(LifetimeSystem))]
    public class CollisionSystem : JobComponentSystem
    {
        private BuildPhysicsWorld physicsWorld;
        private StepPhysicsWorld stepPhysicsWorld;

        protected override void OnCreate()
        {
            base.OnCreate();
            physicsWorld = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<BuildPhysicsWorld>();
            stepPhysicsWorld = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<StepPhysicsWorld>();
        }

        [BurstCompile]
        private struct CollisionJob : ITriggerEventsJob
        {
            public ComponentDataFromEntity<DestroyableDataComponent> destroyables;
            public void Execute(TriggerEvent triggerEvent)
            {
                if (destroyables.HasComponent(triggerEvent.EntityA))
                {
                    var entityToDestroy = destroyables[triggerEvent.EntityA];
                    entityToDestroy.ShouldBeDestroyed = true;
                    destroyables[triggerEvent.EntityA] = entityToDestroy;
                }
                if (destroyables.HasComponent(triggerEvent.EntityB))
                {
                    var entityToDestroy = destroyables[triggerEvent.EntityB];
                    entityToDestroy.ShouldBeDestroyed = true;
                    destroyables[triggerEvent.EntityB] = entityToDestroy;
                }
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var destroyablesComponents = GetComponentDataFromEntity<DestroyableDataComponent>();
            var job = new CollisionJob
            {
                destroyables = destroyablesComponents
            };
            var jobHandle = job.Schedule(stepPhysicsWorld.Simulation, ref physicsWorld.PhysicsWorld, inputDeps);
            jobHandle.Complete();
            return jobHandle;
        }

    }
}