using Components.Data;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Random = UnityEngine.Random;
using UnityEngine;

namespace Monobehaviours
{
    public class BootStrap : MonoBehaviour
    {
        private EntityManager _entityManager;
        private float _currentTimerSinceLastAsteroidSpawn;
        private float _currentTimerSinceLastUfoSpawn;

        public Entity AsteroidPrefabs;
        public Entity UfoPrefabs;  
        public Transform[] SpawnPositions;
        public float AsteroidSpawnFrequency = 2f;
        public float UfoSpawnFrequency = 6f;

        private void Awake()
        {
            _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        }

        private void Start()
        {
            _entityManager.CreateEntity(typeof(InputDataComponent));
            
        }

        private void SpawnAsteroid()
        {
            var buffer = _entityManager.GetBuffer<AsteroidBufferElement>(AsteroidPrefabs);
            //Spawning always large asteroids
            var newAsteroid = _entityManager.Instantiate(buffer[0].Value);

            var randomSpawnPositionIndex = Random.Range(0, SpawnPositions.Length);
            var spawnPosition = SpawnPositions[randomSpawnPositionIndex].position;

            _entityManager.SetComponentData(newAsteroid, new Translation()
            {
                Value = spawnPosition
            });

            var randomMoveVelocity = math.normalize(new float3(Random.Range(-.8f, .8f), Random.Range(-.8f, .8f), 0));
            var randomRotation = Random.value;

            _entityManager.SetComponentData(newAsteroid, new MovementDataComponent()
            {
                PreviousPosition = spawnPosition,
                CurrentVelocity = randomMoveVelocity,
                CurrentTurnAngle = randomRotation
            });
            _entityManager.SetComponentData(newAsteroid, new RandomDataComponent
            {
                Random = Unity.Mathematics.Random.CreateFromIndex((uint)Time.time)
            });
        }

        private void SpawnUfo()
        {
            var buffer = _entityManager.GetBuffer<UfoBufferElement>(UfoPrefabs);
            //Spawning randomly an ufo
            Entity newUfo;
            if(Random.Range(0f, 1f) < 0.5f)
            {
                newUfo = _entityManager.Instantiate(buffer[0].Value);
            }
            else
            {
                newUfo = _entityManager.Instantiate(buffer[1].Value);
            }

            var randomSpawnPositionIndex = Random.Range(0, SpawnPositions.Length);
            var spawnPosition = SpawnPositions[randomSpawnPositionIndex].position;

            _entityManager.SetComponentData(newUfo, new Translation()
            {
                Value = spawnPosition
            });

            var randomMoveVelocity = math.normalize(new float3(Random.Range(-.8f, .8f), Random.Range(-.8f, .8f), 0));
            _entityManager.SetComponentData(newUfo, new MovementDataComponent()
            {
                PreviousPosition = spawnPosition,
                CurrentVelocity = randomMoveVelocity
            });

            _entityManager.SetComponentData(newUfo, new RandomDataComponent
            {
                Random = Unity.Mathematics.Random.CreateFromIndex((uint)Time.time)
            });
        }

        private void Update()
        {
            _currentTimerSinceLastAsteroidSpawn += Time.deltaTime;
            _currentTimerSinceLastUfoSpawn += Time.deltaTime;

            if (_currentTimerSinceLastAsteroidSpawn > AsteroidSpawnFrequency)
            {
                _currentTimerSinceLastAsteroidSpawn = 0;
                SpawnAsteroid();
            }
            if (_currentTimerSinceLastUfoSpawn > UfoSpawnFrequency)
            {
                _currentTimerSinceLastUfoSpawn = 0;
                SpawnUfo();
            }

        }
    }
}