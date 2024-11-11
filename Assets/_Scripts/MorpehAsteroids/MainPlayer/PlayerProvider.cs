using MorpehAsteroids.Signals;
using Scellecs.Morpeh.Providers;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using Zenject;

namespace MorpehAsteroids.MainPlayer 
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class PlayerProvider : MonoProvider<Player>
    {
        public Transform Transform { get; private set; }
        
        public void Initialize(SignalBus signalBus) 
        {
            signalBus.Fire<PlayerHealthChangedSignal>(new(GetData().Health));
        }

        private void Awake()
        {
            Transform = transform;
        }
    }
}