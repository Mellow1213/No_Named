using UnityEngine;

namespace InputSystemAssets
{
    public class PlayerMoveController : MonoBehaviour
    {
        [Header("Player")] [Tooltip("Move speed of the character in m/s")]
        public float MoveSpeed = 10.0f;

        [Tooltip("Move speed of the character in m/s")]
        public float SprintSpeed = 20.0f;

        [Space(10)] [Tooltip("The height the player can jump")]
        public float JumpHeight = 1.2f;

        [Space(10)]
        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        public float JumpTimeout = 0.50f;

        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        public float FallTimeout = 0.15f;

        [Tooltip("Useful for rough ground")]
        public float GroundedOffset = -0.14f;

        [Header("Player Grounded")]
        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        public bool Grounded = true;

        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        public float GroundedRadius = 0.28f;

        [Tooltip("What layers the character uses as ground")]
        public LayerMask GroundLayers;

        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        public float Gravity = -15.0f;

        // Player
        private CharacterController _controller;
        private Animator _animator;
        private InputSystem _inputSystem;

        private bool _hasAnimator;
        private float _speed;
        private float _targetRotation = 0.0f;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;

        // timeout deltatime
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        // Start is called before the first frame update
        void Start()
        {
            _controller = GetComponent<CharacterController>();
            _hasAnimator = TryGetComponent(out _animator);
            _inputSystem = GetComponent<InputSystem>();
        }

        // Update is called once per frame
        void Update()
        {
            _hasAnimator = TryGetComponent(out _animator);
            JumpAndGravity();
            GroundedCheck();
            Move();
        }

        private void GroundedCheck()
        {
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
                transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
                QueryTriggerInteraction.Ignore);
        }

        // Player Move
        private void Move()
        {
            float targetSpeed = _inputSystem.sprint ? SprintSpeed : MoveSpeed;

            if (_inputSystem.move == Vector2.zero)
            {
                targetSpeed = 0.0f;
            }

            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
            Vector3 _moveDirection = new Vector3(_inputSystem.move.x, _verticalVelocity, _inputSystem.move.y);
            
            // _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
            //                  new Vector3(_inputSystem.move.x, _verticalVelocity, _inputSystem.move.y) * Time.deltaTime);
            
            _moveDirection = transform.TransformDirection(_moveDirection).normalized * targetSpeed;
            _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) + _moveDirection * Time.deltaTime);

        }

        private void JumpAndGravity()
        {
            if (Grounded)
            {
                _fallTimeoutDelta = FallTimeout;

                if (_verticalVelocity < 0.2f)
                {
                    _verticalVelocity = -2f;
                }

                // Jump
                if (_inputSystem.jump && _jumpTimeoutDelta <= 0.0f)
                {
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                    // if (_hasAnimator)
                    // {
                    //     에니메이션
                    // }
                }

                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                _jumpTimeoutDelta = JumpTimeout;

                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    // if (_hasAnimator)
                    // {
                    //     에니메이션
                    // }
                }

                _inputSystem.jump = false;
            }

            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
        }
    }
}