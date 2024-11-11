using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Scellecs.Morpeh;
using Zenject;
using MorpehAsteroids.Signals;
using MorpehAsteroids.MainPlayer;
using MorpehAsteroids.MainPlayer.Shooting;

namespace MorpehAsteroids.Input 
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(InputSystem))]
    public sealed class InputSystem : UpdateSystem
    {
        //Must equal the difference between Camera's Y coordinate and Player's Y coordinate
        private const float MouseZCoordinate = 8f;

        private InputAsset _inputAsset;
        private Entity _inputComponentEntity;
        private SignalBus _signalBus;
        private PlayerProvider _playerProvider;

        private float _currentTimer = 0f;
        private float _attackCoolDown;

        public void Initialize(SignalBus signalBus, PlayerProvider playerProvider,
            float attackCoolDown)
        {
            _signalBus = signalBus;
            _playerProvider = playerProvider;
            _attackCoolDown = attackCoolDown;
        }

        public override void OnAwake()
        {
            _inputAsset = new();
            _inputAsset.Enable();

            if (World.Filter.With<InputComponent>().Build().IsEmpty() == true)
            {
                _inputComponentEntity = World.CreateEntity();
                _inputComponentEntity.AddComponent<InputComponent>();
                return;
            }

            _inputComponentEntity = World.Filter.With<InputComponent>().Build().First();
        }

        public override void OnUpdate(float deltaTime)
        {
            SetMovementInput();
            SetAttackInput(deltaTime);
            SetWeaponChangeInput();
        }

        public override void Dispose()
        {
            _inputAsset.Disable();
            _inputAsset.Dispose();
        }

        private void SetMovementInput()
        {
            ref InputComponent inputComponent = ref _inputComponentEntity.GetComponent<InputComponent>();
            Vector2 currentDirectionInput = _inputAsset.PCMap.Move.ReadValue<Vector2>();
            Vector2 currentMousePosition = _inputAsset.PCMap.MousePosition.ReadValue<Vector2>();

            inputComponent.MovementDirection = currentDirectionInput;
            inputComponent.MouseScreenPosition = new(currentMousePosition.x, currentMousePosition.y,
                MouseZCoordinate);
        }

        private void SetAttackInput(float deltaTime)
        {
            if (_inputAsset.PCMap.Attack.ReadValue<float>() > 0f && _currentTimer <= 0f)
            {
                _signalBus.Fire(new SpawnProjectileSignal(typeof(RedProjectileProvider),
                    _playerProvider.Transform));
                _currentTimer = _attackCoolDown;
            }

            if (_currentTimer > 0f)
            {
                _currentTimer -= deltaTime;
            }
        }

        private void SetWeaponChangeInput()
        {
            float currentMouseScroll = _inputAsset.PCMap.ChangeWeapon.ReadValue<Vector2>().y;

            if (currentMouseScroll == 0f)
                return;

            _signalBus.Fire(new MouseScrolledSignal(currentMouseScroll > 0));
        }
    }
}