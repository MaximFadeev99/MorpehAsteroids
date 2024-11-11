using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using Scellecs.Morpeh;
using Zenject;

namespace MorpehAsteroids.Enemies 
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Initializers/" + nameof(EnemyInitializer))]
    public sealed class EnemyInitializer : Initializer
    {
        private AsteroidSpawner _asteroidSpawner;
        private MovementSetUpSystem _movementSetUpSystem;
        private EnemyCountControlSystem _enemyCountControlSystem;

        [Inject(Id = "CB1")] private Transform _closeBoundary1;
        [Inject(Id = "CB2")] private Transform _closeBoundary2;
        [Inject(Id = "FB1")] private Transform _farBoundary1;
        [Inject(Id = "FB2")] private Transform _farBoundary2;

        private readonly List<Entity> _spawnedAsteroids = new();

        [Inject]
        private void Construct(AsteroidSpawner asteroidSpawner, MovementSetUpSystem movementSetUpSystem,
            EnemyCountControlSystem enemyCountControlSystem)
        {
            _asteroidSpawner = asteroidSpawner;
            _movementSetUpSystem = movementSetUpSystem;
            _enemyCountControlSystem = enemyCountControlSystem;
        }

        public override void OnAwake()
        {
            _movementSetUpSystem.Initialize(_closeBoundary1.position, _closeBoundary2.position,
            _farBoundary1.position, _farBoundary2.position);
            _asteroidSpawner.Initialize();
            _enemyCountControlSystem.Initialize(7, _asteroidSpawner);
            _asteroidSpawner.AsteroidSpawned += OnAsteroidSpawned;
        }

        public override void Dispose()
        {
            _asteroidSpawner.AsteroidSpawned -= OnAsteroidSpawned;
            foreach (Entity entity in _spawnedAsteroids)
            {
                ref Asteroid asteroid = ref entity.GetComponent<Asteroid>();
                Addressables.ReleaseInstance(asteroid.GameObject);
            }

            _spawnedAsteroids.Clear();
            _asteroidSpawner.Dispose();
        }

        private void OnAsteroidSpawned(AsteroidProvider spawnedAsteroid)
        {
            if (_spawnedAsteroids.Contains(spawnedAsteroid.Entity))
                return;

            _spawnedAsteroids.Add(spawnedAsteroid.Entity);
            spawnedAsteroid.Destroyed += OnAsteroidDestroyed;
        }

        private void OnAsteroidDestroyed(AsteroidProvider destroyedAsteroid)
        {
            if (_spawnedAsteroids.Contains(destroyedAsteroid.Entity) == false)
                return;

            destroyedAsteroid.Destroyed -= OnAsteroidDestroyed;
            _spawnedAsteroids.Remove(destroyedAsteroid.Entity);
        }
    }
}