using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Monobehaviours
{
    public class UfosPrefabs : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
    {
        public GameController GameController;
        public GameObject[] UfosObjects;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var buffer = dstManager.AddBuffer<UfoBufferElement>(entity);
            foreach (var asteroid in UfosObjects)
            {
                buffer.Add(new UfoBufferElement()
                {
                    Value = conversionSystem.GetPrimaryEntity(asteroid)
                });
            }
            GameController.UfoPrefabs = entity;
        }

        public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
        {
            referencedPrefabs.AddRange(UfosObjects);
        }
    }

    public struct UfoBufferElement : IBufferElementData
    {
        public Entity Value;
    }
}