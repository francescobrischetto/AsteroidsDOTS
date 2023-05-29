using Components;
using Components.Commands;
using Components.Data;
using Components.Tags;
using Unity.Collections;
using Unity.Entities;

namespace Systems
{
    [UpdateAfter(typeof(InputSystem))]
    public class PlayerControlSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var array = GetEntityQuery(typeof(InputDataComponent)).ToComponentDataArray<InputDataComponent>(Allocator.TempJob);
            var inputData = array[0];
            
            Entities.WithAll<PlayerTagComponent>().ForEach((ref MovementCmdComponent movCmd, ref ShootCmdComponent shotCmd) =>
            {
                // movement input command mapping
                var turnLeft = inputData.InputArrowLeft ? 1 : 0; 
                var turnRight = inputData.InputArrowRight ? 1 : 0;
                movCmd.TurnCommand = turnLeft - turnRight;
                movCmd.ThrustCommand = inputData.InputArrowUp ? 1 : 0;
                // shoot input command mapping
                shotCmd.ShootCommand = inputData.InputAction;
            }).ScheduleParallel();

            array.Dispose();
        }
    }
}
