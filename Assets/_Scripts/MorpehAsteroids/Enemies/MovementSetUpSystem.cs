using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Scellecs.Morpeh;

namespace MorpehAsteroids.Enemies 
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(MovementSetUpSystem))]
    public sealed class MovementSetUpSystem : UpdateSystem
    {
        private Vector3 _closeBoundary1;
        private Vector3 _closeBoundary2;
        private Vector3 _farBoundary1;
        private Vector3 _farBoundary2;

        private Filter _flyingEnemyEntities;
        private float _maxFlyDistance;
        private float _minZLimit;
        private float _maxZLimit;

        public override void OnAwake() {}

        public void Initialize(Vector3 closeBoundary1, Vector3 closeBoundary2,
            Vector3 farBoundary1, Vector3 farBoundary2)
        {
            if (_flyingEnemyEntities != null)
                return;

            _flyingEnemyEntities = World.Filter.With<FlyingEnemy>().Build();
            _closeBoundary1 = closeBoundary1;
            _closeBoundary2 = closeBoundary2;
            _farBoundary1 = farBoundary1;
            _farBoundary2 = farBoundary2;
            _maxFlyDistance = Vector3.Distance(_farBoundary1, _farBoundary2);
            _minZLimit = _farBoundary1.x - _closeBoundary1.x;
        }

        public override void OnUpdate(float _)
        {
            if (_flyingEnemyEntities == null)
                return;

            foreach (Entity entity in _flyingEnemyEntities)
            {
                ref FlyingEnemy flyingEnemy = ref entity.GetComponent<FlyingEnemy>();

                if (flyingEnemy.IsMoving)
                    continue;

                SetUpMovementParameters(ref flyingEnemy);
            }
        }

        private void SetUpMovementParameters(ref FlyingEnemy flyingEnemy)
        {
            Vector3 spawnPosition = GetRandomStartPosition();

            flyingEnemy.Transform.position = spawnPosition;
            flyingEnemy.Destination = GetRandomFlyDestination(spawnPosition);
            flyingEnemy.IsMoving = true;
            flyingEnemy.WasWithinScreenBoundaries = false;
            flyingEnemy.IsWithinScreenBoundaries = false;
        }

        private Vector3 GetRandomStartPosition()
        {
            Vector3 randomPosition = new()
            {
                x = Random.Range(_farBoundary1.x, _farBoundary2.x),
                y = 2f
            };

            if (randomPosition.x < _minZLimit && randomPosition.x > _maxZLimit)
            {
                randomPosition.z = Random.Range(_farBoundary1.z, _farBoundary2.z);
            }
            else
            {
                randomPosition.z = Random.Range(0, 2) == 0 ?
                    Random.Range(_closeBoundary1.z, _farBoundary1.z) :
                    Random.Range(_closeBoundary2.z, _farBoundary2.z);
            }

            return randomPosition;
        }

        private Vector3 GetRandomFlyDestination(Vector3 spawnPosition)
        {
            Vector3 randomPointWithinCloseBoundaries = new()
            {
                x = Random.Range(_closeBoundary1.x, _closeBoundary2.x),
                y = 2f,
                z = Random.Range(_closeBoundary1.z, _closeBoundary2.z),
            };

            return (randomPointWithinCloseBoundaries - spawnPosition).normalized * _maxFlyDistance;
        }
    }
}