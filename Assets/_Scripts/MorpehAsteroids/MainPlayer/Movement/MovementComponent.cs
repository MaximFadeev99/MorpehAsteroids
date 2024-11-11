using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

namespace MorpehAsteroids.MainPlayer.Movement
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct MovementComponent : IComponent
    {
        public Rigidbody Rigidbody;
        public float Speed;
        public float VelocityLimit;
    }
}