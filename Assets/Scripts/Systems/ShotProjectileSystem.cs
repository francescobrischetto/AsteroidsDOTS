using Components.Data;
using Components.Stats;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

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
                , ref WeaponDataComponent weaponData, in WeaponStatsComponent weaponStats, in MovementDataComponent movData, in Translation translation, in Rotation rotation) =>
            {
                weaponData.FireTimer += deltaTime * weaponStats.FireRateMultiplier;
                
                if (!weaponData.IsShooting || weaponData.FireTimer < weaponStats.FireRate) return;
                
                weaponData.FireTimer = 0;
                
                for (int i = 0; i < weaponStats.NumProjectilesToSpawn; ++i)
                {
                    Entity newProjectile = ecb.Instantiate(entityInQueryIndex, weaponStats.ProjectileEntity);
                    
                    Vector3 projectileDirection = math.mul(rotation.Value, weaponData.ShootDirection);
                    projectileDirection.Normalize();
                    
                    ecb.SetComponent(entityInQueryIndex, newProjectile, new Translation { Value = translation.Value + i * (float3)projectileDirection * weaponStats.SpaceBetweenProjectiles });
                    ecb.SetComponent(entityInQueryIndex, newProjectile, new Rotation { Value = rotation.Value });
                    ecb.SetComponent(entityInQueryIndex, newProjectile, new MovementDataComponent
                    {
                        CurrentVelocity = projectileDirection * weaponStats.ProjectileSpeed
                    });
                }
            }).ScheduleParallel();
            
            _endSimulationECBS.AddJobHandleForProducer(Dependency);
        }
    }
}