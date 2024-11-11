using MorpehAsteroids.Damage;
using MorpehAsteroids.MainPlayer;
using MorpehAsteroids.MainPlayer.Shooting;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using System;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace MorpehAsteroids.Enemies 
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class AsteroidProvider : MonoProvider<Asteroid>
    {
        private GameObject _gameObject;

        public Action<AsteroidProvider> Destroyed;

        private void Awake()
        {
            _gameObject = gameObject;
            GetData().GameObject = _gameObject;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out PlayerProvider playerProvider)) 
            {
                CreateDamageComponent(playerProvider);
                return;
            }

            if (collision.gameObject.TryGetComponent(out ProjectileProvider _)) 
            {
                DestroyAsteroid();
                return;
            }
        }

        private void CreateDamageComponent(PlayerProvider playerProvider) 
        {
            Entity newEntity = World.Default.CreateEntity();
            ref DamageComponent newDamageComponent = ref newEntity.AddComponent<DamageComponent>();
            newDamageComponent.DamagedEntityId = playerProvider.Entity.ID;
            newDamageComponent.DamageToApply = GetData().CollisionDamage;
        }

        private void DestroyAsteroid() 
        {
            Destroy(_gameObject);
        }

        private void OnDestroy()
        {
            Destroyed?.Invoke(this);
        }
    }
}