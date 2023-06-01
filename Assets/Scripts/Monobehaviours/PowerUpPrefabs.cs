using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Monobehaviours
{
    public class PowerUpPrefabs : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
    {
        public GameController GameController;
        public GameObject[] PowerUpObjects;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var buffer = dstManager.AddBuffer<PowerUpBufferElement>(entity);
            foreach (var powerUp in PowerUpObjects)
            {
                buffer.Add(new PowerUpBufferElement()
                {
                    Value = conversionSystem.GetPrimaryEntity(powerUp)
                });
            }
            GameController.PowerUpPrefabs = entity;
        }

        public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
        {
            referencedPrefabs.AddRange(PowerUpObjects);
        }
    }

    public struct PowerUpBufferElement : IBufferElementData
    {
        public Entity Value;
    }
}