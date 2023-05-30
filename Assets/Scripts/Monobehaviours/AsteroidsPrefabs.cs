using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Monobehaviours
{
    public class AsteroidsPrefabs : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
    {
        public BootStrap BootStrap;
        public GameObject[] AsteroidsObjects;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var buffer = dstManager.AddBuffer<AsteroidBufferElement>(entity);
            foreach (var asteroid in AsteroidsObjects)
            {
                buffer.Add(new AsteroidBufferElement()
                {
                    Value = conversionSystem.GetPrimaryEntity(asteroid)
                });
            }
            BootStrap.AsteroidPrefabs = entity;
        }

        public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
        {
            referencedPrefabs.AddRange(AsteroidsObjects);
        }
    }

    public struct AsteroidBufferElement : IBufferElementData
    {
        public Entity Value;
    }
}