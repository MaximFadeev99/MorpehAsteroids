using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Scellecs.Morpeh;
using MorpehAsteroids.MainPlayer;
using Scellecs.Morpeh.Collections;
using Zenject;
using MorpehAsteroids.Signals;

namespace MorpehAsteroids.Damage 
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(DamageSystem))]
    public sealed class DamageSystem : UpdateSystem
    {
        private readonly FastList<Entity> _entitiesToDestroy = new();

        private Filter _damageComponentEntities;
        private SignalBus _signalBus;

        public void Initialize(SignalBus signalBus) 
        {
            _signalBus = signalBus;
        }

        public override void OnAwake()
        {
            _damageComponentEntities = World.Filter.With<DamageComponent>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (Entity entity in _damageComponentEntities)
            {
                DamageComponent damageComponent = entity.GetComponent<DamageComponent>();

                if (World.TryGetEntity(damageComponent.DamagedEntityId, out Entity damagedEntity) == false)
                    continue;

                ref Player player = ref damagedEntity.GetComponent<Player>();

                player.Health -= damageComponent.DamageToApply;
                _entitiesToDestroy.Add(entity);
                _signalBus.Fire(new PlayerHealthChangedSignal(player.Health));
                entity.Dispose();
            }
        }
    }
}