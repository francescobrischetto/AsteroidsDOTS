using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Monobehaviours
{
    public class ShipPrefab : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
    {
        public GameController GameController;
        public GameObject ShipObject;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity, new ShipElementDataComponent
            {
                Value = conversionSystem.GetPrimaryEntity(ShipObject)
            });
            
            GameController.ShipPrefab = entity;
        }

        public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
        {
            referencedPrefabs.Add(ShipObject);
        }
    }

    public struct ShipElementDataComponent : IComponentData
    {
        public Entity Value;
    }

}