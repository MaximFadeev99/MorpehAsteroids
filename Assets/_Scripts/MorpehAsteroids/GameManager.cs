using MorpehAsteroids.MainPlayer;
using UnityEngine;
using Zenject;

namespace MorpehAsteroids
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private SystemInstaller _systemInstaller;
        [SerializeField] private PlayerSetter _playerSetter;

        private SignalBus _signalBus;

        [Inject]
        private void Construct(SignalBus signalBus) 
        {
            _signalBus = signalBus;
        }

        private void Awake()
        {
            _playerSetter.Intialize(_signalBus);
            _systemInstaller.Initialize(_signalBus, _playerSetter.PlayerProvider, _playerSetter.SpawnProjectileFunc);
        }

        private void OnApplicationQuit()
        {
            _playerSetter.Dispose();
        }
    }
}