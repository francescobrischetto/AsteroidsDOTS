using Components.Data;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Random = UnityEngine.Random;
using UnityEngine;

namespace Monobehaviours
{
    public class GameController : MonoBehaviour
    {
        private EntityManager _entityManager;
        private float _currentTimerSinceLastAsteroidSpawn;
        private float _currentTimerSinceLastUfoSpawn;
        private float _currentTimerSinceLastPowerUpSpawn;

        public Entity AsteroidPrefabs;
        public Entity UfoPrefabs;  
        public Entity PowerUpPrefabs;
        public Entity LastPowerUpSpawned;
        public Transform[] SpawnPositions;
        public float AsteroidSpawnFrequency = 2f;
        public float UfoSpawnFrequency = 6f;
        public float PowerUpSpawnFrequency = 2f;
        private void Awake()
        {
            _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        }

        private void Start()
        {
            _entityManager.CreateEntity(typeof(InputDataComponent));
            SpawnAsteroid();
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
            int randomIndex = Random.Range(0, buffer.Length);
            Entity newUfo = _entityManager.Instantiate(buffer[randomIndex].Value);

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

        private void SpawnPowerUp()
        {
            var buffer = _entityManager.GetBuffer<PowerUpBufferElement>(PowerUpPrefabs);
            
            int randomIndex = Random.Range(0, buffer.Length);
            LastPowerUpSpawned = _entityManager.Instantiate(buffer[randomIndex].Value);

            _entityManager.SetComponentData(LastPowerUpSpawned, new Translation()
            {
                Value = new Vector3(Random.Range(-4f, 4f),Random.Range(-4f, 4f), 0)
            });
        }
        
        private void Update()
        {
            _currentTimerSinceLastAsteroidSpawn += Time.deltaTime;
            _currentTimerSinceLastUfoSpawn += Time.deltaTime;
            _currentTimerSinceLastPowerUpSpawn += Time.deltaTime;

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
            if (_currentTimerSinceLastPowerUpSpawn > PowerUpSpawnFrequency)
            {
                _currentTimerSinceLastPowerUpSpawn = 0;
                // spawn new powerup if there isn't already one in game
                if (!_entityManager.Exists(LastPowerUpSpawned))
                {
                    SpawnPowerUp();
                }
            }
        }
    }
}