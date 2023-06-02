using Components.Data;
using Components.Stats;
using Components.Tags;
using System;
using Unity.Entities;
using Unity.Jobs;
using Utils;

namespace Systems
{
    [UpdateBefore(typeof(CollisionSystem))]
    public class PowerUpSystem : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem _endSimulationECBS;
        public event EventHandler OnPowerUpLost;
        public struct OnPowerUpLostEvent : IComponentData { public int Value; }

        private DOTSEvents_SameFrame<OnPowerUpLostEvent> dotsEvents;
        protected override void OnCreate()
        {
            base.OnCreate();
            _endSimulationECBS = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
            dotsEvents = new DOTSEvents_SameFrame<OnPowerUpLostEvent>(World);
        }
        protected override void OnUpdate()
        {
            var deltaTime = Time.DeltaTime;
            DOTSEvents_SameFrame<OnPowerUpLostEvent>.EventTrigger eventTrigger = dotsEvents.GetEventTrigger();
            var ecb = _endSimulationECBS.CreateCommandBuffer().AsParallelWriter();
            ComponentDataFromEntity<WeaponStatsComponent> weaponStatsEntities = GetComponentDataFromEntity<WeaponStatsComponent>(true);
            ComponentDataFromEntity<DestroyableDataComponent> destroyableEntities = GetComponentDataFromEntity<DestroyableDataComponent>(true);
            ComponentDataFromEntity<PlayerTagComponent> playersEntities = GetComponentDataFromEntity<PlayerTagComponent>(true);


            JobHandle jobHandle = Entities.ForEach((Entity entity, int entityInQueryIndex, ref PowerUpDataComponent powerUpData, ref PowerUpStatsComponent powerUpStats) =>
            {
                powerUpData.ElapsedTime += deltaTime;

                if (powerUpData.ElapsedTime < powerUpData.MaxTime)
                {
                    //If player still exists we need to give him powerups effects
                    if (playersEntities.HasComponent(powerUpData.TargetEntity))
                    {
                        //Powerups effects
                        switch (powerUpStats.Type)
                        {
                            case PowerUpType.Invulnerable:
                                DestroyableDataComponent destroyableDataComponent = destroyableEntities[powerUpData.TargetEntity];
                                ecb.SetComponent(powerUpData.TargetEntity.Index, powerUpData.TargetEntity, new DestroyableDataComponent()
                                {
                                    IsInvulnerable = true,
                                    ShouldBeDestroyed = destroyableDataComponent.ShouldBeDestroyed
                                });
                                break;
                            case PowerUpType.TriShot:
                                WeaponStatsComponent weaponStatsComponent = weaponStatsEntities[powerUpData.TargetEntity];
                                ecb.SetComponent(powerUpData.TargetEntity.Index, powerUpData.TargetEntity, new WeaponStatsComponent()
                                {
                                    ProjectileEntity = weaponStatsComponent.ProjectileEntity,
                                    ProjectileSpeed = weaponStatsComponent.ProjectileSpeed,
                                    AngleBetweenProjectiles = weaponStatsComponent.AngleBetweenProjectiles,
                                    FireRate = weaponStatsComponent.FireRate,
                                    FireRateMultiplier = weaponStatsComponent.FireRateMultiplier,
                                    NumProjectilesToSpawn = 3
                                });
                                break;
                            case PowerUpType.DoubleFireRate:
                                WeaponStatsComponent weaponStatsComponent2 = weaponStatsEntities[powerUpData.TargetEntity];
                                ecb.SetComponent(powerUpData.TargetEntity.Index, powerUpData.TargetEntity, new WeaponStatsComponent()
                                {
                                    ProjectileEntity = weaponStatsComponent2.ProjectileEntity,
                                    ProjectileSpeed = weaponStatsComponent2.ProjectileSpeed,
                                    AngleBetweenProjectiles = weaponStatsComponent2.AngleBetweenProjectiles,
                                    FireRate = weaponStatsComponent2.FireRate,
                                    FireRateMultiplier = 2,
                                    NumProjectilesToSpawn = weaponStatsComponent2.NumProjectilesToSpawn,
                                });
                                break;
                        }
                    }
                    
                }
                else
                {
                    ecb.DestroyEntity(entityInQueryIndex,entity);
                    //If player still exists we need to remove from him powerups effects
                    if (playersEntities.HasComponent(powerUpData.TargetEntity))
                    {
                        eventTrigger.TriggerEvent(entityInQueryIndex);
                        switch (powerUpStats.Type)
                        {
                            //Powerups effects reverted
                            case PowerUpType.Invulnerable:
                                DestroyableDataComponent destroyableDataComponent = destroyableEntities[powerUpData.TargetEntity];
                                ecb.SetComponent(powerUpData.TargetEntity.Index, powerUpData.TargetEntity, new DestroyableDataComponent()
                                {
                                    IsInvulnerable = false,
                                    ShouldBeDestroyed = destroyableDataComponent.ShouldBeDestroyed
                                });
                                break;
                            case PowerUpType.TriShot:
                                WeaponStatsComponent weaponStatsComponent = weaponStatsEntities[powerUpData.TargetEntity];
                                ecb.SetComponent(powerUpData.TargetEntity.Index, powerUpData.TargetEntity, new WeaponStatsComponent()
                                {
                                    ProjectileEntity = weaponStatsComponent.ProjectileEntity,
                                    ProjectileSpeed = weaponStatsComponent.ProjectileSpeed,
                                    AngleBetweenProjectiles = weaponStatsComponent.AngleBetweenProjectiles,
                                    FireRate = weaponStatsComponent.FireRate,
                                    FireRateMultiplier = weaponStatsComponent.FireRateMultiplier,
                                    NumProjectilesToSpawn = 1,
                                });
                                break;
                            case PowerUpType.DoubleFireRate:
                                WeaponStatsComponent weaponStatsComponent2 = weaponStatsEntities[powerUpData.TargetEntity];
                                ecb.SetComponent(powerUpData.TargetEntity.Index, powerUpData.TargetEntity, new WeaponStatsComponent()
                                {
                                    ProjectileEntity = weaponStatsComponent2.ProjectileEntity,
                                    ProjectileSpeed = weaponStatsComponent2.ProjectileSpeed,
                                    AngleBetweenProjectiles = weaponStatsComponent2.AngleBetweenProjectiles,
                                    FireRate = weaponStatsComponent2.FireRate,
                                    FireRateMultiplier = 1,
                                    NumProjectilesToSpawn = weaponStatsComponent2.NumProjectilesToSpawn,
                                });
                                break;
                        }
                    }
                }

            }).WithReadOnly(weaponStatsEntities).WithReadOnly(destroyableEntities).WithReadOnly(playersEntities).ScheduleParallel(Dependency);
            dotsEvents.CaptureEvents(eventTrigger, jobHandle, (OnPowerUpLostEvent onPowerUpLostEvent) => {
                OnPowerUpLost?.Invoke(this, EventArgs.Empty);
            });
            Dependency = JobHandle.CombineDependencies(jobHandle, Dependency);
            _endSimulationECBS.AddJobHandleForProducer(Dependency);
        }
    }
}
