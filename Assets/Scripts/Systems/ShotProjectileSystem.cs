using Components.Data;
using Components.Stats;
using System;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Utils;

namespace Systems
{
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    public class ShotProjectileSystem : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem _endSimulationECBS;
        public event EventHandler OnShoot;
        public struct OnShootEvent : IComponentData { public int Value; }

        private DOTSEvents_NextFrame<OnShootEvent> dotsEvents;

        protected override void OnCreate()
        {
            base.OnCreate();
            _endSimulationECBS = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
            dotsEvents = new DOTSEvents_NextFrame<OnShootEvent>(World);
        }

        protected override void OnUpdate()
        {
            var deltaTime = Time.DeltaTime;
            DOTSEvents_NextFrame<OnShootEvent>.EventTrigger eventTrigger = dotsEvents.GetEventTrigger();
            var ecb = _endSimulationECBS.CreateCommandBuffer().AsParallelWriter();

            JobHandle jobHandle = Entities.ForEach((Entity entity, int entityInQueryIndex
                , ref WeaponDataComponent weaponData, in WeaponStatsComponent weaponStats, in MovementDataComponent movData, in Translation translation, in Rotation rotation) =>
            {
                weaponData.FireTimer += deltaTime * weaponStats.FireRateMultiplier;
                
                //Firing is limited by Fire Rate 
                if (!weaponData.IsShooting || weaponData.FireTimer < weaponStats.FireRate) return;
                
                weaponData.FireTimer = 0;
                
                for (int i = 0; i < weaponStats.NumProjectilesToSpawn; ++i)
                {
                    //If projectiles are more than 1 (eg. Trishots) we need to adjust
                    //their position/rotation/direction accordingly, considering the angle between them
                    Entity newProjectile = ecb.Instantiate(entityInQueryIndex, weaponStats.ProjectileEntity);
                    int rotationMultiplier = (int)math.ceil(i / 2.0f);
                    rotationMultiplier = i % 2 == 0 ? -rotationMultiplier : rotationMultiplier;
                    quaternion projectileRotation = math.mul(rotation.Value, quaternion.Euler(0, 0, rotationMultiplier * weaponStats.AngleBetweenProjectiles));
                    Vector3 projectileDirection = math.mul(projectileRotation, weaponData.ShootDirection);
                    projectileDirection.Normalize();

                    ecb.SetComponent(entityInQueryIndex, newProjectile, new Translation { Value = translation.Value });
                    ecb.SetComponent(entityInQueryIndex, newProjectile, new Rotation { Value = projectileRotation});
                    ecb.SetComponent(entityInQueryIndex, newProjectile, new MovementDataComponent
                    {
                        CurrentVelocity = projectileDirection * weaponStats.ProjectileSpeed
                    });
                    
                }
                eventTrigger.TriggerEvent(entityInQueryIndex);

            }).ScheduleParallel(Dependency);
            dotsEvents.CaptureEvents(eventTrigger, jobHandle, (OnShootEvent onShootProjectileEvent) => {
                OnShoot?.Invoke(this, EventArgs.Empty);
            });
            Dependency = JobHandle.CombineDependencies(jobHandle, Dependency);
            _endSimulationECBS.AddJobHandleForProducer(Dependency);
        }
    }
}