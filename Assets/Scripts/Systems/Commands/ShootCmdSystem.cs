using Components.Commands;
using Components.Data;
using Unity.Entities;

namespace Systems.Commands
{
    public class ShootCmdSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((ref WeaponDataComponent weaponData, in ShootCmdComponent shootCmd) =>
            {
                weaponData.IsShooting = shootCmd.ShootCommand;
            }).ScheduleParallel();
        }
    }
}