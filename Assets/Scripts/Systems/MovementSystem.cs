using Components.Data;
using Components.Stats;
using Systems.Commands;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Systems
{
    [UpdateAfter(typeof(MovementCmdSystem))]
    public class MovementSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var deltaTime = Time.DeltaTime;   
            
            Entities.ForEach((ref Translation translation, ref Rotation rotation, ref MovementDataComponent movData, in MovementStatsComponent movStats) =>
            {
                movData.PreviousPosition = translation.Value;
                // Calculate the rotation delta
                float angle = movData.CurrentTurnAngle * deltaTime;
                quaternion rotationDelta = quaternion.RotateZ(angle);
                // Add the rotation delta to the existing rotation value
                rotation.Value = math.mul(rotation.Value, rotationDelta);

                float3 currentDirection = movData.CurrentVelocity;
                math.normalize(currentDirection);
                // Get the forward direction
                float3 forwardDirection = math.mul(rotation.Value, math.up());
                movData.CurrentVelocity -= currentDirection * movStats.MovementFriction * deltaTime;
                movData.CurrentVelocity += forwardDirection * movData.CurrentAcceleration * deltaTime;
                // Apply traslation
                translation.Value += movData.CurrentVelocity * deltaTime;

            }).ScheduleParallel();
        }
    }
}