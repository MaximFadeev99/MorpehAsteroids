using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Scellecs.Morpeh;

namespace MorpehAsteroids.Enemies 
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(EnemyFlyingSystem))]
    public sealed class EnemyFlyingSystem : FixedUpdateSystem
    {
        private Filter _flyingEnemyEntities;

        public override void OnAwake()
        {
            _flyingEnemyEntities = World.Filter.With<FlyingEnemy>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (Entity entity in _flyingEnemyEntities)
            {
                FlyingEnemy flyingEnemy = entity.GetComponent<FlyingEnemy>();

                if (flyingEnemy.IsMoving == false)
                {
                    flyingEnemy.Rigidbody.velocity = Vector3.zero;
                    continue;
                }

                flyingEnemy.Rigidbody.velocity =
                    (flyingEnemy.Destination - flyingEnemy.Rigidbody.position).normalized * flyingEnemy.Speed;
            }
        }
    }
}