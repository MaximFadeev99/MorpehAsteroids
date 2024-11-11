using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

namespace MorpehAsteroids.Enemies 
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct FlyingEnemy : IComponent
    {
        public Rigidbody Rigidbody;
        public Transform Transform;
        public float Speed;

        [HideInInspector] public Vector3 Destination;
        [HideInInspector] public bool IsWithinScreenBoundaries;
        [HideInInspector] public bool WasWithinScreenBoundaries;
        [HideInInspector] public bool IsMoving;
    }
}