using MorpehAsteroids.Signals;
using MorpehAsteroids.Enemies;
using UnityEngine;
using Zenject;

namespace MorpehAsteroids.DI 
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private AsteroidSpawner _asteroidSpawner;
        [SerializeField] private MovementSetUpSystem _movementSetUpSystem;
        [SerializeField] private EnemyCountControlSystem _enemyCountControlSystem;
        [SerializeField] private EnemyInitializer _enemyInitializer;

        [SerializeField] private Transform _closeBoundary1;
        [SerializeField] private Transform _closeBoundary2;
        [SerializeField] private Transform _farBoundary1;
        [SerializeField] private Transform _farBoundary2;

        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);

            Container.DeclareSignal<PlayerHealthChangedSignal>().OptionalSubscriber();
            Container.DeclareSignal<SpawnProjectileSignal>().OptionalSubscriber();
            Container.DeclareSignal<MouseScrolledSignal>().OptionalSubscriber();

            Container.BindInstances(_asteroidSpawner, _movementSetUpSystem, _enemyCountControlSystem);
            Container.BindInstance(_closeBoundary1).WithId("CB1");
            Container.BindInstance(_closeBoundary2).WithId("CB2");
            Container.BindInstance(_farBoundary1).WithId("FB1");
            Container.BindInstance(_farBoundary2).WithId("FB2");

            Container.Inject(_enemyInitializer);
        }
    }
}