using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

namespace MorpehAsteroids.MainPlayer.Shooting 
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct Projectile : IComponent
    {
        public Rigidbody Rigidbody;
        public float FlyingSpeed;
        public int Damage;
        public float MaxFlyTime;

        [HideInInspector]
        public GameObject GameObject;

        [HideInInspector]
        public Vector3 FlyingDirection;

        [HideInInspector]
        public float ActualFlyTime;
    }
}