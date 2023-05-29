using Components;
using Components.Commands;
using Components.Data;
using Components.Tags;
using Systems.Commands;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
namespace Systems
{
    [UpdateBefore(typeof(MovementCmdSystem))]
    public class UfoControllerSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var deltaTime = Time.DeltaTime;
            var random = GetSingleton<RandomDataComponent>();


            Entities.WithAll<UfoTagComponent>().ForEach((ref MovementCmdComponent movCmd, 
                ref ShootCmdComponent shotCmd, ref UfoIADataComponent ufoIA, ref WeaponDataComponent weaponData) =>
            {
                movCmd.TurnCommand = 0;
                movCmd.ThrustCommand = 1;
                weaponData.ShootDirection = math.normalize(new float3(random.Random.NextFloat(0f,1f), random.Random.NextFloat(0f,1f), 0));
                shotCmd.ShootCommand = true;
                ufoIA.CurrentTimeSinceDirectionChangeAttempt += deltaTime;
                if(ufoIA.CurrentTimeSinceDirectionChangeAttempt >= ufoIA.AttemptChangeDirectionTimer)
                {
                    if(random.Random.NextFloat(0,1) < ufoIA.ChangeDirectionRate)
                    {
                        movCmd.TurnCommand = 1;
                    }
                    ufoIA.CurrentTimeSinceDirectionChangeAttempt = 0;
                }
            }).ScheduleParallel();

        }
    }
}
