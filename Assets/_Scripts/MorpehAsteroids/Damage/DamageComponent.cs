using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace MorpehAsteroids.Damage 
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct DamageComponent : IComponent
    {
        public EntityId DamagedEntityId;
        public int DamageToApply;
    }
}