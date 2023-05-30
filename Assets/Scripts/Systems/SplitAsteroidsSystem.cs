using Components.Data;
using Components.Tag;
using Components.Tags;
using Monobehaviours;
using System.Linq;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using static UnityEngine.EventSystems.EventTrigger;

namespace Systems
{
    [UpdateAfter(typeof(MovementSystem))]
    public class SplitAsteroidsSystem : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem _endSimulationECBS;
        private EntityManager _entityManager;
        protected override void OnCreate()
        {
            base.OnCreate();
            _endSimulationECBS = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
            _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        }

        protected override void OnUpdate()
        {
            var deltaTime = Time.DeltaTime;
            var ecb = _endSimulationECBS.CreateCommandBuffer().AsParallelWriter();
            var asteroidPrefab = GetSingletonEntity<AsteroidsPrefabTagComponent>();
            var lookup = GetBufferFromEntity<AsteroidBufferElement>(true);
            var asteroidPrefabsBuffer = lookup[asteroidPrefab];


            Entities.WithAll<AsteroidTagComponent>().ForEach(
                (Entity entity, int entityInQueryIndex,
                ref RandomDataComponent random,
                in DestroyableDataComponent destroyableData,
                in AsteroidTagComponent asteroidTag,
                in Translation translation, 
                in Rotation rotation
                ) =>
            {
                if (destroyableData.ShouldBeDestroyed)
                {
                    Entity nextPrefabEntity = GetNextAsteroidPrefab(asteroidPrefabsBuffer, asteroidTag.Type);
                    if (nextPrefabEntity != Entity.Null)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            Entity newProjectile = ecb.Instantiate(entityInQueryIndex, nextPrefabEntity);
                            ecb.SetComponent(entityInQueryIndex, newProjectile, new Translation { Value = translation.Value });
                            ecb.SetComponent(entityInQueryIndex, newProjectile, new Rotation { Value = rotation.Value });
                            ecb.SetComponent(entityInQueryIndex, newProjectile, new MovementDataComponent
                            {
                                CurrentVelocity = math.mul(rotation.Value, new float3(random.Random.NextFloat2(-3f, 3f), 0f))
                            });
                            ecb.SetComponent(entityInQueryIndex, newProjectile, new RandomDataComponent
                            {
                                Random = Random.CreateFromIndex((uint)entityInQueryIndex)
                            });
                        }
                    }
                }

            }).WithReadOnly(asteroidPrefabsBuffer).ScheduleParallel();
            _endSimulationECBS.AddJobHandleForProducer(Dependency);
        }
        
        //This function returns the correct next asteroid prefab based on the current asteroid type that will be destroyed
        private static Entity GetNextAsteroidPrefab(DynamicBuffer<AsteroidBufferElement> asteroidPrefabsBuffer, AsteroidType asteroydType)
        {
            
            switch (asteroydType)
            {   
                case AsteroidType.Large:    return asteroidPrefabsBuffer[1].Value;
                case AsteroidType.Medium:   return asteroidPrefabsBuffer[2].Value;
                case AsteroidType.Small:    
                default: return Entity.Null;
            }
        }
    }
}