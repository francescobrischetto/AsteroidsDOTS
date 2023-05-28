using Unity.Entities;
using UnityEngine;
using Components.Data;

namespace Components.Authorings
{
    public class ScreenWrapperDataComponentAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        [SerializeField] SpriteRenderer spriteRenderer;
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity, new ScreenWrapperDataComponent()
            {
                Bounds = spriteRenderer.bounds.extents.magnitude
            });

        }
    }
}
