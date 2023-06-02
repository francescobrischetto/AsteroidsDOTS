using Components.Data;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Random = UnityEngine.Random;
using UnityEngine;
using Systems;
using System.Collections;
using Components.Stats;
using System;

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
        public Entity ShipPrefab;

        public Transform[] SpawnPositions;
        public float AsteroidSpawnFrequency = 2f;
        public float UfoSpawnFrequency = 6f;
        public float PowerUpSpawnFrequency = 2f;
        private Entity shipEntity;

        private void Awake()
        {
            _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<DestroyableSystem>().OnShipDestroyed += RespawnShip;
            World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<DestroyableSystem>().OnObjectDestroyed += OnObjectDestroyed;
            World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<ShotProjectileSystem>().OnShoot += OnShoot;
            World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<CollisionSystem>().OnPowerUpPicked += OnPowerUpPicked;
            World.DefaultGameObjectInjectionWorld.GetOrCreateSystem < PowerUpSystem > ().OnPowerUpLost += OnPowerUpLost;
        }

        private void Start()
        {
            _entityManager.CreateEntity(typeof(InputDataComponent));
            shipEntity = SpawnShipAtPosition(Vector3.zero);
            GrantShipInvulnerability(shipEntity, 1.5f);
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
                Random = new Unity.Mathematics.Random((uint)Random.Range(int.MinValue, int.MaxValue))
            }) ;
        }

        private void SpawnPowerUp()
        {
            var buffer = _entityManager.GetBuffer<PowerUpBufferElement>(PowerUpPrefabs);
            
            int randomIndex = Random.Range(0, buffer.Length);
            LastPowerUpSpawned = _entityManager.Instantiate(buffer[randomIndex].Value);

            _entityManager.SetComponentData(LastPowerUpSpawned, new Translation()
            {
                Value = new Vector3(Random.Range(-5f, 5f),Random.Range(-4f, 4f), 0)
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

            if (Input.GetKeyUp(KeyCode.X))
            {
                HyperspaceTravel();
            }
        }

        private Entity SpawnShipAtPosition(Vector3 spawnPosition)
        {
            var shipPrefabReference = _entityManager.GetComponentData<ShipElementDataComponent>(ShipPrefab);
            var playerShip = _entityManager.Instantiate(shipPrefabReference.Value);
            _entityManager.SetComponentData(playerShip, new Translation
            {
                Value = spawnPosition
            });
            return playerShip;
        }

        private void RespawnShip(object sender, System.EventArgs e)
        {
            SoundManager.PlaySound(SoundManager.Sound.LoseLife);
            StartCoroutine(SpawnShipAfterDelay(1));
        }

        private void GrantShipInvulnerability(Entity playerShip, float seconds)
        {
            EntityArchetype powerUpArchetype = _entityManager.CreateArchetype(
                typeof(PowerUpDataComponent),
                typeof(PowerUpStatsComponent)
            );
            Entity powerUpEntity = _entityManager.CreateEntity(powerUpArchetype);
            _entityManager.SetComponentData(powerUpEntity, new PowerUpDataComponent
            {
                MaxTime = seconds,
                ElapsedTime = 0,
                TargetEntity = playerShip
            });
            _entityManager.SetComponentData(powerUpEntity, new PowerUpStatsComponent
            {
                Type = PowerUpType.Invulnerable
            });
            //OnPowerUpPicked(this, EventArgs.Empty);

        }

        private IEnumerator SpawnShipAfterDelay(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            shipEntity = SpawnShipAtPosition(Vector3.zero);
            GrantShipInvulnerability(shipEntity, 1.5f);
        }

        private void HyperspaceTravel()
        {
            if(_entityManager.Exists(shipEntity))
            {
                _entityManager.SetComponentData(shipEntity, new Translation
                {
                    Value = new Vector3(Random.Range(-6.6f, 6.6f), Random.Range(-4.9f, 4.9f), 0)
                });
                SoundManager.PlaySound(SoundManager.Sound.HyperSpaceTravel);
            }
        }

        private void OnShoot(object sender, System.EventArgs e)
        {
            SoundManager.PlaySound(SoundManager.Sound.Shoot );
        }
        private void OnObjectDestroyed(object sender, System.EventArgs e)
        {
            SoundManager.PlaySound(SoundManager.Sound.ObjectDestroyed);
        }
        private void OnPowerUpPicked(object sender, System.EventArgs e)
        {
            SoundManager.PlaySound(SoundManager.Sound.PowerUpPicked);
        }
        private void OnPowerUpLost(object sender, System.EventArgs e)
        {
            SoundManager.PlaySound(SoundManager.Sound.PowerUpLost);
        }
    }
}