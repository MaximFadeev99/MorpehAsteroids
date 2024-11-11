using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Scellecs.Morpeh;

namespace MorpehAsteroids.Enemies 
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(EnemyCountControlSystem))]
    public sealed class EnemyCountControlSystem : UpdateSystem
    {
        private int _targetAsteroidCount;
        private AsteroidSpawner _asteroidSpawner;
        private Filter _asteroidEntities;

        public override void OnAwake()
        {
        }

        internal void Initialize(int targetAsteroidCount, AsteroidSpawner asteroidSpawner)
        {
            _targetAsteroidCount = targetAsteroidCount;
            _asteroidSpawner = asteroidSpawner;
            _asteroidEntities = World.Filter.With<Asteroid>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            if (_asteroidEntities == null)
                return;

            int currrentAsteroidCount = _asteroidEntities.GetLengthSlow() + _asteroidSpawner.QueuedSpawns;

            if (currrentAsteroidCount >= _targetAsteroidCount)
                return;

            int amountToSpawn = _targetAsteroidCount - currrentAsteroidCount;

            _asteroidSpawner.SpawnAsteroids(amountToSpawn, 3f);
        }
    }
}