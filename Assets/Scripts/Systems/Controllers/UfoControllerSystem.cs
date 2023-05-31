using Components.Commands;
using Components.Data;
using Components.Tags;
using Systems.Commands;
using Unity.Entities;

namespace Systems.Controllers
{
    [UpdateBefore(typeof(MovementCmdSystem))]
    public class UfoControllerSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var deltaTime = Time.DeltaTime;

            Entities.WithAll<UfoTagComponent>().ForEach((ref MovementCmdComponent movCmd, 
                ref ShootCmdComponent shotCmd, ref UfoIADataComponent ufoIA, ref WeaponDataComponent weaponData, 
                ref RandomDataComponent  random) =>
            {
                movCmd.TurnCommand = 0;
                movCmd.ThrustCommand = 1;
                weaponData.ShootDirection = random.Random.NextFloat3Direction();
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
