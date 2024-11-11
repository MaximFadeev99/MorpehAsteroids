using System;
using UnityEngine;

namespace MorpehAsteroids.Signals
{
    public class SpawnProjectileSignal
    {
        public readonly Type Type;
        public readonly Transform AttackPoint;

        public SpawnProjectileSignal(Type type, Transform attackPoint)
        {
            Type = type;
            AttackPoint = attackPoint;
        }
    }
}