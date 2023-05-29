using Components.Data;
using Components.Tags;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Systems
{
    [UpdateAfter(typeof(MovementSystem))]
    public class SplitAsteroidsSystem : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem _endSimulationECBS;
        protected override void OnCreate()
        {
            base.OnCreate();
            _endSimulationECBS = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var deltaTime = Time.DeltaTime;
            var ecb = _endSimulationECBS.CreateCommandBuffer().AsParallelWriter();
            var asteroidPrefab = GetSingleton<AsteroidsPrefabDataComponent>();
            var randomData = GetSingleton<RandomDataComponent>();

            Entities.WithAll<AsteroidTagComponent>().ForEach(
                (Entity entity, int entityInQueryIndex,
                in DestroyableDataComponent destroyableData,
                in AsteroidTagComponent asteroidTag,
                in Translation translation, 
                in Rotation rotation) =>
            {
                if (destroyableData.ShouldBeDestroyed)
                {
                    Entity nextPrefabEntity = GetNextAsteroidPrefab(asteroidPrefab, asteroidTag.Type);
                    if (nextPrefabEntity != Entity.Null)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            Entity newProjectile = ecb.Instantiate(entityInQueryIndex, nextPrefabEntity);
                            ecb.SetComponent(entityInQueryIndex, newProjectile, new Translation { Value = translation.Value });
                            ecb.SetComponent(entityInQueryIndex, newProjectile, new Rotation { Value = rotation.Value });
                            ecb.SetComponent(entityInQueryIndex, newProjectile, new MovementDataComponent
                            {
                                CurrentVelocity = math.mul(rotation.Value, new float3(randomData.Random.NextFloat2(-3f, 3f), 0f))
                            });
                        }
                    }
                }

            }).ScheduleParallel();
            
            _endSimulationECBS.AddJobHandleForProducer(Dependency);
        }
        
        //This function returns the correct next asteroid prefab based on the current asteroid type that will be destroyed
        private static Entity GetNextAsteroidPrefab(AsteroidsPrefabDataComponent asteroidPrefab, AsteroidType asteroydType)
        {
            switch (asteroydType)
            {   
                case AsteroidType.Large:    return asteroidPrefab.MediumAsteroidPrefab;
                case AsteroidType.Medium:   return asteroidPrefab.SmallAsteroidPrefab;
                case AsteroidType.Small:    
                default: return Entity.Null;
            }
        }
    }
}