using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Scellecs.Morpeh;

namespace MorpehAsteroids.Enemies 
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(ScreenBoundaryCheckingSystem))]
    public sealed class ScreenBoundaryCheckingSystem : LateUpdateSystem
    {
        private Camera _mainCamera;
        private Filter _flyingEnemyEntities;

        public override void OnAwake()
        {
            _mainCamera = Camera.main;
            _flyingEnemyEntities = World.Filter.With<FlyingEnemy>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (Entity flyingEnemyEntity in _flyingEnemyEntities)
            {
                ref FlyingEnemy flyingEnemy = ref flyingEnemyEntity.GetComponent<FlyingEnemy>();

                Vector2 viewPortPosition = _mainCamera.WorldToViewportPoint(flyingEnemy.Transform.position);

                if (viewPortPosition.x < -0.2f || viewPortPosition.x > 1.2f ||
                    viewPortPosition.y < -0.2f || viewPortPosition.y > 1.2f)
                {
                    flyingEnemy.IsWithinScreenBoundaries = false;
                }
                else
                {
                    flyingEnemy.IsWithinScreenBoundaries = true;
                    flyingEnemy.WasWithinScreenBoundaries = true;
                }

                if (flyingEnemy.WasWithinScreenBoundaries && flyingEnemy.IsWithinScreenBoundaries == false)
                    flyingEnemy.IsMoving = false;
            }
        }
    }
}