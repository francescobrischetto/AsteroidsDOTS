using Components.Data;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Mathematics;

namespace Systems
{
    public class WrappingSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var screenInfo = GetSingleton<ScreenInfoDataComponent>();
            Entities.WithAll<ScreenWrapperDataComponent>().ForEach((ref ScreenWrapperDataComponent screenWrapper, ref Translation translation) =>
            {
                if(screenWrapper.IsOffScreenLeft)
                {
                    translation.Value = new float3((screenWrapper.Bounds + screenInfo.Width) * 0.5f, translation.Value.y, 0);
                }

                if (screenWrapper.IsOffScreenRight)
                {
                    translation.Value = new float3(- (screenWrapper.Bounds + screenInfo.Width) * 0.5f, translation.Value.y, 0);
                }

                if (screenWrapper.IsOffScreenBottom)
                {
                    translation.Value = new float3(translation.Value.x, (screenWrapper.Bounds + screenInfo.Height) * 0.5f, 0);
                }

                if (screenWrapper.IsOffScreenTop)
                {
                    translation.Value = new float3(translation.Value.x, - (screenWrapper.Bounds + screenInfo.Height) * 0.5f, 0);
                }

                screenWrapper.IsOffScreenTop = false;
                screenWrapper.IsOffScreenBottom = false;
                screenWrapper.IsOffScreenLeft = false;
                screenWrapper.IsOffScreenRight = false;

            }).ScheduleParallel();
        }
    }
}
