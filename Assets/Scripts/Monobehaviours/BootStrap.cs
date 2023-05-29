using Components.Data;
using Unity.Entities;
using UnityEngine;

namespace Monobehaviours
{
    public class BootStrap : MonoBehaviour
    {
        private EntityManager _entityManager;

        private void Awake()
        {
            _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        }

        private void Start()
        {
            Entity entity = _entityManager.CreateEntity(typeof(RandomDataComponent));
            _entityManager.SetComponentData(entity, new RandomDataComponent
            {
                Random = new Unity.Mathematics.Random((uint)(Time.realtimeSinceStartup * 100))
            }) ;
            _entityManager.CreateEntity(typeof(InputDataComponent));
            
        }
    }
}