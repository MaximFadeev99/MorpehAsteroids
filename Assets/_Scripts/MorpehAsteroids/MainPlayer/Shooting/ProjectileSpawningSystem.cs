using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Zenject;
using MorpehAsteroids.Signals;
using System;
using Cysharp.Threading.Tasks;

namespace MorpehAsteroids.MainPlayer.Shooting 
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(ProjectileSpawningSystem))]
    public sealed class ProjectileSpawningSystem : UpdateSystem
    {
        private SignalBus _signalBus;
        private Func<Vector3, Quaternion, UniTask<ProjectileProvider>> _spawnProjectileFunc;

        public void Initialize(Func<Vector3, Quaternion, UniTask<ProjectileProvider>> spawnProjectileFunc,
            SignalBus signalBus)
        {
            _spawnProjectileFunc = spawnProjectileFunc;
            _signalBus = signalBus;
            _signalBus.Subscribe<SpawnProjectileSignal>(OnProjectileSpawnSignal);
        }

        public override void OnAwake(){}

        public override void OnUpdate(float _){}

        public override void Dispose()
        {
            _signalBus.TryUnsubscribe<SpawnProjectileSignal>(OnProjectileSpawnSignal);
        }

        private async void OnProjectileSpawnSignal(SpawnProjectileSignal signal) 
        {
            ProjectileProvider spawnedProjectileProvider = await _spawnProjectileFunc.
                Invoke(signal.AttackPoint.position,signal.AttackPoint.rotation);
            spawnedProjectileProvider.SetPositionAndRotation(signal.AttackPoint.position, 
                signal.AttackPoint.rotation);
            LaunchProjectile(spawnedProjectileProvider, signal.AttackPoint.forward);
            
        }

        private void LaunchProjectile(ProjectileProvider projectileProvider, Vector3 flyDirection)
        {
            ref Projectile projectile = ref projectileProvider.GetData();
            projectile.FlyingDirection = flyDirection;
        }
    }
}