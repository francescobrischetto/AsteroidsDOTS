using Components;
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
            _entityManager.CreateEntity(typeof(InputDataComponent));
        }
    }
}