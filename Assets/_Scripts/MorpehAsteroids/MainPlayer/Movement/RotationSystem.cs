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
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(RotationSystem))]
    public sealed class RotationSystem : FixedUpdateSystem
    {
        private const float PlayerYCoordinate = 2f;

        [SerializeField] private VectorPID _dumpController;
        [SerializeField] private VectorPID _pushController;

        private Filter _rotatingEntities;
        private Entity _inputComponentEntity;
        private Camera _mainCamera;

        public override void OnAwake()
        {
            _rotatingEntities = World.Filter.With<RotationComponent>().Build();
            _inputComponentEntity = World.Filter.With<InputComponent>().Build().First();
            _mainCamera = Camera.main;
        }

        public override void OnUpdate(float deltaTime)
        {
            InputComponent inputComponent = _inputComponentEntity.GetComponent<InputComponent>();
            Vector3 mouseWorldPosition = _mainCamera.ScreenToWorldPoint(inputComponent.MouseScreenPosition);
            mouseWorldPosition.y = PlayerYCoordinate;

            foreach (Entity entity in _rotatingEntities)
            {
                RotationComponent rotationComponent = entity.GetComponent<RotationComponent>();

                AddTorque(rotationComponent.Rigidbody, rotationComponent.Transform, mouseWorldPosition,
                    rotationComponent.TorqueSpeed);
            }
        }

        private void AddTorque(Rigidbody rigidbody, Transform transform, Vector3 targetPosition, 
            float torqueSpeed)
        {
            Vector3 dumpingTorque = _dumpController
                .Update(-rigidbody.angularVelocity, Time.deltaTime);

            rigidbody.AddTorque(dumpingTorque);

            Vector3 pushingTorqueDirection = Vector3.Cross
                (transform.forward, targetPosition - transform.position).normalized;
            Vector3 pushingTorque = _pushController
                .Update(pushingTorqueDirection * torqueSpeed, Time.deltaTime);

            rigidbody.AddTorque(pushingTorque);
        }
    }
}