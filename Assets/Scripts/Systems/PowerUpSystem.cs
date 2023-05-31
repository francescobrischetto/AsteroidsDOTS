using Components.Data;
using Components.Stats;
using Unity.Entities;

namespace Systems
{
    [UpdateBefore(typeof(CollisionSystem))]
    public class PowerUpSystem : SystemBase
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
            ComponentDataFromEntity<WeaponStatsComponent> weaponStatsEntities = GetComponentDataFromEntity<WeaponStatsComponent>(true);
            ComponentDataFromEntity<DestroyableDataComponent> destroyableEntities = GetComponentDataFromEntity<DestroyableDataComponent>(true);

            Entities.ForEach((Entity entity, int entityInQueryIndex, ref PowerUpDataComponent powerUpData, ref PowerUpStatsComponent powerUpStats) =>
            {
                powerUpData.ElapsedTime += deltaTime;

                if (powerUpData.ElapsedTime < powerUpData.MaxTime)
                {
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
                                SpaceBetweenProjectiles = weaponStatsComponent.SpaceBetweenProjectiles,
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
                                SpaceBetweenProjectiles = weaponStatsComponent2.SpaceBetweenProjectiles,
                                FireRate = weaponStatsComponent2.FireRate,
                                FireRateMultiplier = 2,
                                NumProjectilesToSpawn = weaponStatsComponent2.NumProjectilesToSpawn,
                            });
                            break;
                    }
                }
                else
                {
                    ecb.DestroyEntity(entityInQueryIndex,entity);
                    switch (powerUpStats.Type)
                    {
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
                                SpaceBetweenProjectiles = weaponStatsComponent.SpaceBetweenProjectiles,
                                FireRate = weaponStatsComponent.FireRate,
                                FireRateMultiplier = weaponStatsComponent.FireRateMultiplier,
                                NumProjectilesToSpawn = 1,
                            });
                            break;
                        case  PowerUpType.DoubleFireRate:
                            WeaponStatsComponent weaponStatsComponent2 = weaponStatsEntities[powerUpData.TargetEntity];
                            ecb.SetComponent(powerUpData.TargetEntity.Index, powerUpData.TargetEntity, new WeaponStatsComponent()
                            {
                                ProjectileEntity = weaponStatsComponent2.ProjectileEntity,
                                ProjectileSpeed = weaponStatsComponent2.ProjectileSpeed,
                                SpaceBetweenProjectiles = weaponStatsComponent2.SpaceBetweenProjectiles,
                                FireRate = weaponStatsComponent2.FireRate,
                                FireRateMultiplier = 1,
                                NumProjectilesToSpawn = weaponStatsComponent2.NumProjectilesToSpawn,
                            });
                            break;
                    }
                }

            }).WithReadOnly(weaponStatsEntities).WithReadOnly(destroyableEntities).ScheduleParallel();

            _endSimulationECBS.AddJobHandleForProducer(Dependency);
        }
    }
}
