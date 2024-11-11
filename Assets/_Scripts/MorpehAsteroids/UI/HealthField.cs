using MorpehAsteroids.Signals;
using TMPro;
using UnityEngine;
using Zenject;

namespace MorpehAsteroids.UI
{
    public class HealthField : MonoBehaviour
    {
        private const string Caption = "Player Health: ";

        private SignalBus _signalBus;
        private TMP_Text _field;

        public void Initialize(SignalBus signalBus) 
        {
            _signalBus = signalBus;
            _field = GetComponent<TMP_Text>();
            _signalBus.Subscribe<PlayerHealthChangedSignal>(OnPlayerHealthChanged);
        }

        private void OnDestroy()
        {
            _signalBus.TryUnsubscribe<PlayerHealthChangedSignal>(OnPlayerHealthChanged);
        }

        private void OnPlayerHealthChanged(PlayerHealthChangedSignal signal) 
        {
            _field.text =  Caption + signal.CurrentHealth.ToString();
        }
    }
}