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
        
        // [Tooltip("Acceleration and deceleration")]
        // public float SpeedChangeRate = 10.0f;

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
        private float _animationBlend;

        private bool _hasAnimator;
        private float _targetRotation = 0.0f;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;

        // timeout deltatime
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;
        
        // animation
        private int _animSpeed;
        private int _animGrounded;
        private int _animJump;
        private int _animFreeFall;
        private int _animMotionSpeed;
        
        // Start is called before the first frame update
        void Start()
        {
            _controller = GetComponent<CharacterController>();
            _inputSystem = GetComponent<InputSystem>();
            _hasAnimator = TryGetComponent(out _animator);
            AnimationID();
        }

        // Update is called once per frame
        void Update()
        {
            JumpAndGravity();
            GroundedCheck();
            Move();
            
        }
        
        // Player AnimationID
        private void AnimationID()
        {
            _animSpeed = Animator.StringToHash("Speed");
            _animGrounded = Animator.StringToHash("Grounded");
            _animJump = Animator.StringToHash("Jump");
            _animFreeFall = Animator.StringToHash("FreeFall");
            _animMotionSpeed = Animator.StringToHash("MotionSpeed");

        }
        
        // Player Grounde Check
        private void GroundedCheck()
        {
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
                transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
                QueryTriggerInteraction.Ignore);

            if (_hasAnimator)
            {
                _animator.SetBool(_animGrounded, Grounded);
            }
        }

        // Player Move
        private void Move()
        {
            float targetSpeed = _inputSystem.sprint ? SprintSpeed : MoveSpeed;

            if (_inputSystem.move == Vector2.zero)
            {
                targetSpeed = 0.0f;
            }
            
            Vector3 inputDirection = new Vector3(_inputSystem.move.x, 0.0f, _inputSystem.move.y);
            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * inputDirection;
            Vector3 verticalDirection = Vector3.up * _verticalVelocity;

            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * 10f);
            if (_animationBlend < 0.01f)
            {
                _animationBlend = 0f;
            }
            
            float inputMagnitude = _inputSystem.analogMovement ? _inputSystem.move.magnitude : 1f;
            

            _controller.Move(targetDirection.normalized * (targetSpeed * Time.deltaTime) + verticalDirection * Time.deltaTime);

            if (_hasAnimator)
            {
                _animator.SetFloat(_animSpeed, _animationBlend);
                _animator.SetFloat(_animMotionSpeed, inputMagnitude);
            }
            
        }
        
        // Player Jump and Gravity
        private void JumpAndGravity()
        {
            if (Grounded)
            {
                _fallTimeoutDelta = FallTimeout;

                if (_hasAnimator)
                {
                    _animator.SetBool(_animJump, false);
                    _animator.SetBool(_animFreeFall, false);
                }

                if (_verticalVelocity < 0.2f)
                {
                    _verticalVelocity = -2f;
                }
                

                // Jump
                if (_inputSystem.jump && _jumpTimeoutDelta <= 0.0f)
                {
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
                    
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animJump, true);
                    }
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
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animFreeFall, true);
                    }
                }

                _inputSystem.jump = false;
            }

            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
        }

        // 에니메이션 이벤트 제거 못해서 남겨둠
        private void OnLand(AnimationEvent animationEvent)
        {
            
        }

        private void OnFootstep(AnimationEvent animationEvent)
        {
            
        }
    }
}