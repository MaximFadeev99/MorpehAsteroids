using Unity.IL2CPP.CompilerServices;

namespace MorpehAsteroids.MainPlayer.Shooting 
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class RedProjectileProvider : ProjectileProvider{}
}