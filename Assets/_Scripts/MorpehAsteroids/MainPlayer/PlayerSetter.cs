using Cysharp.Threading.Tasks;
using MorpehAsteroids.MainPlayer.Shooting;
using MorpehAsteroids.Signals;
using MorpehAsteroids.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace MorpehAsteroids.MainPlayer
{
    [Serializable]
    public class PlayerSetter 
    {
        [SerializeField] private int _targetAppearanceIndex;
        [SerializeField] private int _targetProjectileIndex;
        [SerializeField] private PlayerSpawner _playerSpawner;
        [SerializeField] private HealthField _healthField;
        [field: SerializeField] public PlayerProvider PlayerProvider { get; private set; }

        private SignalBus _signalBus;
        private int _currentProjectileIndex = 0;

        public Func<Vector3, Quaternion, UniTask<ProjectileProvider>> SpawnProjectileFunc 
            => _playerSpawner.SpawnProjectile;

        public void Intialize(SignalBus signalBus) 
        {
            _signalBus = signalBus;
            _healthField.Initialize(_signalBus);
            PlayerProvider.Initialize(_signalBus);
            _playerSpawner.SetPlayerAppearance(_targetAppearanceIndex, PlayerProvider.transform);
            _playerSpawner.SetTargetProjectileType(_targetProjectileIndex);
            _signalBus.Subscribe<MouseScrolledSignal>(OnMouseScrolled);
        }
        public void Dispose() 
        {
            _signalBus.TryUnsubscribe<MouseScrolledSignal>(OnMouseScrolled);
            _playerSpawner.Dispose();
        }

        private void OnMouseScrolled(MouseScrolledSignal signal)
        {
            if (signal.HasScrolledUp == true &&
                _currentProjectileIndex + 1 <= _playerSpawner.AvailableProjectileCount - 1)
            {
                _currentProjectileIndex++;
                _playerSpawner.SetTargetProjectileType(_currentProjectileIndex);
                return;
            }

            if (signal.HasScrolledUp == true &&
                _currentProjectileIndex + 1 > _playerSpawner.AvailableProjectileCount - 1)
            {
                _currentProjectileIndex = 0;
                _playerSpawner.SetTargetProjectileType(_currentProjectileIndex);
                return;
            }

            if (signal.HasScrolledUp == false && _currentProjectileIndex - 1 >= 0)
            {
                _currentProjectileIndex--;
                _playerSpawner.SetTargetProjectileType(_currentProjectileIndex);
                return;
            }

            if (signal.HasScrolledUp == false && _currentProjectileIndex - 1 < 0)
            {
                _currentProjectileIndex = _playerSpawner.AvailableProjectileCount - 1;
                _playerSpawner.SetTargetProjectileType(_currentProjectileIndex);
                return;
            }
        }
    }
}
