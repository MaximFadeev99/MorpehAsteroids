using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Scellecs.Morpeh;
using MorpehAsteroids.Input;

namespace MorpehAsteroids.MainPlayer.Movement 
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(MovementSystem))]
    public sealed class MovementSystem : FixedUpdateSystem
    {
        private Filter _movingEntities;
        private Entity _inputComponentEntity;
        private InputComponent _inputComponent;

        public override void OnAwake()
        {
            _movingEntities = World.Filter.With<MovementComponent>().Build();
            _inputComponentEntity = World.Filter.With<InputComponent>().Build().First();
        }

        public override void OnUpdate(float deltaTime)
        {
            _inputComponent = _inputComponentEntity.GetComponent<InputComponent>();

            if (_inputComponent.MovementDirection == Vector2.zero)
                return;

            foreach (Entity entity in _movingEntities)
            {
                ref MovementComponent movementComponent = ref entity.GetComponent<MovementComponent>();
                Vector3 addedForce = new Vector3(_inputComponent.MovementDirection.x, 0f, _inputComponent.MovementDirection.y)
                    * movementComponent.Speed;

                if (movementComponent.Rigidbody.velocity.x > movementComponent.VelocityLimit)
                    addedForce.x = 0f;

                if (movementComponent.Rigidbody.velocity.z > movementComponent.VelocityLimit)
                    addedForce.z = 0f;

                movementComponent.Rigidbody.AddForce(addedForce, ForceMode.Force);
            }
        }
    }
}