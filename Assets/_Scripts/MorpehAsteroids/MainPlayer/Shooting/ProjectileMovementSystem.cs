using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Scellecs.Morpeh;
using System.Collections.Generic;

namespace MorpehAsteroids.MainPlayer.Shooting 
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(ProjectileMovementSystem))]
    public sealed class ProjectileMovementSystem : FixedUpdateSystem
    {
        private readonly List<Projectile> _projectilesToDelete = new();
        private Filter _projectiles;

        public override void OnAwake(){}

        public override void OnUpdate(float deltaTime)
        {
            _projectiles = World.Filter.With<Projectile>().Build();

            foreach (Entity entity in _projectiles) 
            {
                ref Projectile projectile = ref entity.GetComponent<Projectile>();

                projectile.Rigidbody.velocity = projectile.FlyingDirection * projectile.FlyingSpeed;
                projectile.ActualFlyTime += deltaTime;

                if (projectile.ActualFlyTime > projectile.MaxFlyTime) 
                {
                    AddProjectileForDeletion(projectile);
                }
            }

            DeleteRegisteredProjectiles();
        }

        private void AddProjectileForDeletion(Projectile projectile) 
        {
            if (_projectilesToDelete.Contains(projectile))
                return;

            _projectilesToDelete.Add(projectile);      
        }

        private void DeleteRegisteredProjectiles() 
        {
            if (_projectilesToDelete.Count == 0) 
                return;

            foreach (Projectile projectile in _projectilesToDelete) 
            {
                Destroy(projectile.GameObject);
            }

            _projectilesToDelete.Clear();
        }
    }
}