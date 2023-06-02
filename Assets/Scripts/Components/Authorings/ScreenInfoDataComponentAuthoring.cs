using Unity.Entities;
using UnityEngine;
using Components.Data;

namespace Components.Authorings
{
    // This entity holds screen Height and Width infomations used for wraparond and other systems
    public class ScreenInfoDataComponentAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        [SerializeField] new Camera camera;
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var bottomLeftCornerpos = camera.ViewportToWorldPoint(new Vector3(0, 0, 0));
            var topRightCornerpos = camera.ViewportToWorldPoint(new Vector3(1, 1, 0));

            var screenHeight = topRightCornerpos.y - bottomLeftCornerpos.y;
            var screenWidth = topRightCornerpos.x - bottomLeftCornerpos.x;

            dstManager.AddComponentData(entity, new ScreenInfoDataComponent()
            {
                Height = screenHeight,
                Width = screenWidth
            });

        }
    }
}
