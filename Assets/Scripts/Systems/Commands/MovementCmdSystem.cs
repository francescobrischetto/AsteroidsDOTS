using Components.Commands;
using Components.Data;
using Components.Stats;
using Unity.Entities;

namespace Systems.Commands
{
    public class MovementCmdSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((ref MovementDataComponent movData, in MovementStatsComponent movStats, 
                in MovementCmdComponent movCmd) =>
            {
                movData.CurrentTurnAngle = movCmd.TurnCommand * movStats.TurnRate;
                movData.CurrentAcceleration = movCmd.ThrustCommand * movStats.Acceleration;
                
            }).ScheduleParallel();
        }
    }
}