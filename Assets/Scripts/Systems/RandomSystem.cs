using Components.Data;
using Unity.Entities;
using Random = Unity.Mathematics.Random;
namespace Systems
{
    public class RandomSystem : SystemBase
    {

        protected override void OnStartRunning()
        {
            Entities.ForEach((Entity e, int entityInQueryIndex, ref RandomDataComponent randomData) =>
            {
                randomData.Random = Random.CreateFromIndex((uint)entityInQueryIndex);
            }).ScheduleParallel();
        }

        protected override void OnUpdate()
        {
        }
    }
}