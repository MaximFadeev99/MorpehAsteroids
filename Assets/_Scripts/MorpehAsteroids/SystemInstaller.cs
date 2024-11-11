using Cysharp.Threading.Tasks;
using MorpehAsteroids.Damage;
using MorpehAsteroids.Input;
using MorpehAsteroids.MainPlayer;
using MorpehAsteroids.MainPlayer.Shooting;
using System;
using UnityEngine;
using Zenject;

namespace MorpehAsteroids
{
    [Serializable]
    internal class SystemInstaller 
    {
        [SerializeField] private InputSystem _inputSystem;
        [SerializeField] private DamageSystem _damageSystem;
        [SerializeField] private ProjectileSpawningSystem _projectileSpawningSystem;
        [SerializeField] private float _attackCooldown;

        internal void Initialize(SignalBus signalBus, PlayerProvider playerProvider,
            Func<Vector3, Quaternion, UniTask<ProjectileProvider>> spawnProjectileFunc) 
        {
            _damageSystem.Initialize(signalBus);
            _projectileSpawningSystem.Initialize(spawnProjectileFunc, signalBus);
            _inputSystem.Initialize(signalBus, playerProvider, _attackCooldown);
        }
    }
}