using Scellecs.Morpeh.Providers;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace MorpehAsteroids.MainPlayer.Shooting 
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public abstract class ProjectileProvider : MonoProvider<Projectile>
    {

        private Transform _transform;
        public GameObject GameObject { get; private set; }

        private void Awake()
        {
            GameObject = gameObject;
            _transform = transform;
            ref Projectile data = ref GetData();
            data.GameObject = GameObject;
        }

        public void SetPositionAndRotation(Vector3 worldPosition, Quaternion worldRotation) 
        {
            _transform.SetPositionAndRotation(worldPosition, worldRotation);
        }

        private void OnCollisionEnter(Collision collision)
        {
            Destroy(GameObject);
        }
    }
}