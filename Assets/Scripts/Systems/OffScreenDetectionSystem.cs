using Components.Data;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

namespace Systems
{
    public class OffScreenDetectionSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var screenInfo = GetSingleton<ScreenInfoDataComponent>();
            Entities.ForEach((ref ScreenWrapperDataComponent screenWrapper, ref MovementDataComponent movData, ref Translation translation) =>
            {
                var previousPosition = movData.PreviousPosition;
                var currentPosition = translation.Value;

                var isMovingLeft = currentPosition.x - previousPosition.x < 0;
                var isMovingRight = currentPosition.x - previousPosition.x > 0;
                var isMovingDown = currentPosition.y - previousPosition.y < 0;
                var isMovingUp = currentPosition.y - previousPosition.y > 0;

                //Those calculations considers the direction of movement and the bounds of the current entity
                screenWrapper.IsOffScreenLeft = currentPosition.x < - (screenInfo.Width + screenWrapper.Bounds) * 0.5f && isMovingLeft;
                screenWrapper.IsOffScreenRight = currentPosition.x > (screenInfo.Width + screenWrapper.Bounds) * 0.5f && isMovingRight;
                screenWrapper.IsOffScreenBottom = currentPosition.y < - (screenInfo.Height + screenWrapper.Bounds) * 0.5f && isMovingDown;
                screenWrapper.IsOffScreenTop = currentPosition.y > (screenInfo.Height + screenWrapper.Bounds) * 0.5f && isMovingUp;

            }).ScheduleParallel();
        }
    }
}
