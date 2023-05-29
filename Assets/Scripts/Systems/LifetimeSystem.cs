using Components.Data;
using Unity.Entities;
using Unity.Jobs;

namespace Systems
{
    public class LifetimeSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var deltaTime = Time.DeltaTime;
            Entities.ForEach((ref LifetimeDataComponent lifetimeData, ref DestroyableDataComponent destroyableData) =>
            {
                lifetimeData.CurrentTimeSinceSpawning += deltaTime;
                if(lifetimeData.CurrentTimeSinceSpawning >= lifetimeData.Lifetime && !destroyableData.ShouldBeDestroyed)
                {
                    destroyableData.ShouldBeDestroyed = true;
                }

            }).ScheduleParallel();
        }
    }
}
