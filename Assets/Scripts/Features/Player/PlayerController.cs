using Data;
using Infrastructure.Services.Interfaces;
using UnityEngine;
using Zenject;

namespace Features.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    public class PlayerController : MonoBehaviour
    {
        private IInputService _inputService;
        private PlayerSettings _playerSettings;
        private Rigidbody2D _rigidbody;
        private Animator _animator;

        private Vector2 _lastMoveDirection;

        private const float MoveDeadZone = 0.1f;


        [Inject]
        private void Construct(
            IInputService inputService,
            PlayerSettings playerSettings)
        {
            _inputService = inputService;
            _playerSettings = playerSettings;
        }

        void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _lastMoveDirection = Vector2.down;
        }

        void FixedUpdate()
        {
            Vector2 input = _inputService.MoveInput;

            bool isMoving = input.sqrMagnitude > MoveDeadZone * MoveDeadZone;

            Vector2 moveDir = isMoving ? input.normalized : Vector2.zero;

            Move(moveDir);

            if (isMoving)
            {
                _lastMoveDirection = moveDir;
            }

            Animate(moveDir, isMoving);
        }


        private void Move(Vector2 direction)
        {
            _rigidbody.velocity = direction * _playerSettings.moveSpeed;
        }

        private void Animate(Vector2 direction, bool isMoving)
        {
            _animator.SetFloat("MoveX", direction.x);
            _animator.SetFloat("MoveY", direction.y);
            _animator.SetBool("IsMoving", isMoving);

            _animator.SetFloat("LastMoveX", _lastMoveDirection.x);
            _animator.SetFloat("LastMoveY", _lastMoveDirection.y);
        }
    }
}