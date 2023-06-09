using Components.Data;
using Components.Tag;
using Components.Tags;
using Monobehaviours;
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
                            //Spawning two new asteroids
                            Entity newAsteroid = ecb.Instantiate(entityInQueryIndex, nextPrefabEntity);
                            ecb.SetComponent(entityInQueryIndex, newAsteroid, new Translation { Value = translation.Value });
                            ecb.SetComponent(entityInQueryIndex, newAsteroid, new Rotation { Value = rotation.Value });
                            ecb.SetComponent(entityInQueryIndex, newAsteroid, new MovementDataComponent
                            {
                                CurrentVelocity = math.mul(rotation.Value, new float3(random.Random.NextFloat2(-3f, 3f), 0f)),
                                CurrentTurnAngle = random.Random.NextFloat(0.1f, 3f)
                            });
                            //Initializing its random component seed
                            ecb.SetComponent(entityInQueryIndex, newAsteroid, new RandomDataComponent
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