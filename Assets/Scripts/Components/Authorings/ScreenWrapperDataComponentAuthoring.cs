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
            //Calculating sprite bounds, useful for wraparound system
            dstManager.AddComponentData(entity, new ScreenWrapperDataComponent()
            {
                Bounds = spriteRenderer.bounds.extents.magnitude
            });

        }
    }
}
