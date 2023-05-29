using Components.Data;
using Components.Stats;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Systems
{
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    public class ShotProjectileSystem : SystemBase
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

            Entities.ForEach((Entity entity, int entityInQueryIndex
                , ref WeaponDataComponent weaponData, in MovementDataComponent movData, in WeaponStatsComponent weaponStats, in Translation translation, in Rotation rotation) =>
            {
                weaponData.FireTimer += deltaTime;
                
                if (!weaponData.IsShooting || weaponData.FireTimer < weaponStats.FireRate) return;
                
                weaponData.FireTimer = 0;
                Entity newProjectile = ecb.Instantiate(entityInQueryIndex, weaponStats.ProjectileEntity);

                ecb.SetComponent(entityInQueryIndex, newProjectile, new Translation { Value = translation.Value });
                ecb.SetComponent(entityInQueryIndex, newProjectile, new Rotation { Value = rotation.Value });
                ecb.SetComponent(entityInQueryIndex, newProjectile, new MovementDataComponent
                {
                    CurrentVelocity = math.mul(rotation.Value, new float3(0f, 1f, 0f)) * weaponStats.ProjectileSpeed + movData.CurrentVelocity
                }) ;

            }).ScheduleParallel();
            
            _endSimulationECBS.AddJobHandleForProducer(Dependency);
        }
    }
}