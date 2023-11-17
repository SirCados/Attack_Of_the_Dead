using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacterController : MonoBehaviour
{
    [SerializeField] private AudioClip[] _footstepSounds;    // an array of footstep sounds that will be randomly selected from.
    [SerializeField] private AudioClip _jumpSound;           // the sound played when character leaves the ground.
    [SerializeField] private AudioClip _landSound;
    [SerializeField] private AudioClip _hitSound;
    [SerializeField] private AudioClip _deathSound;
    [SerializeField] private AudioClip _attackSound;

    public bool IsInputBlockedExternally = false;
    public bool IsGameOver = false;
    public bool IsNewIsland;
    public float SpeedSmoothTime = 0.1f;
    public float TurnSmoothTime = 0.2f;
    public Camera PlayerCamera;
    public float Gravity = -9.8f;
    public float JumpHeight = 1;
    public GameObject Hurtbox;
    public GameObject NewIslandObject;
    bool _isObeyingGravity;    
    private bool _isInputBlockedInternally = false;
    public float SpawnDelay = 0;

    [Range(0, 1)]
    float _airWalk;

    Animator _animator;
    AudioSource _audioSource;
    bool _isAttacking = false;
    bool _isFalling = false;
    bool _isSuperFalling = false;
    bool _isSuperJumping = false;
    CharacterController _characterController;
    PlayerHurtbox _hurtbox;
    float _currentSpeed;
    float _gravityVelocity;
    float _speedSmoothVelocity;
    float _turnSmoothVelocity;
    float _fallTime =0;
    PlayerStats _playerStats;
    Vector2 _movementVector;
    Vector2 _storedMovementVector;

    float _walkSpeed = 4;
    float _runSpeed = 9;

    Transform _cameraTransform;


    // Start is called before the first frame update
    void Start()
    {
        _isObeyingGravity = true;
        IsNewIsland = false;
        _animator = GetComponent<Animator>();
        _hurtbox = Hurtbox.GetComponent<PlayerHurtbox>();
        _cameraTransform = PlayerCamera.transform;
        _characterController = GetComponent<CharacterController>();
        _playerStats = GetComponent<PlayerStats>();
        _audioSource = GetComponent<AudioSource>();
        GetPlayerStats();
    }

    // Update is called once per frame
    void Update()
    {
        Recover();
        CharacterIsFalling();
        MoveCharacter();
        GetPlayerStats();
    }

    private void FixedUpdate()
    {
        CharacterAttack();
    }

    void CheckIfFallenTooFar()
    {
        if(transform.position.y < -5)
        {
            _playerStats.TakeDamage(_playerStats.MaxHealth);
        }
    }

    void CharacterIsFalling()
    {
        if (!_characterController.isGrounded && _isObeyingGravity)
        {
            if(_fallTime == 0)
            {
                _fallTime = Time.time;
            }
            if(_fallTime != 0 && _fallTime + 1 < Time.time)
            {
                _isFalling = true;
                _animator.SetBool("Falling", _isFalling);
            }
            CheckIfFallenTooFar();
        }
        else if((_characterController.isGrounded && _isFalling)|| !_isObeyingGravity)
        {
            _fallTime = 0;
            _isFalling = false; //Just thought to do this, Might be easier to set up everything else like this for animations.
            _animator.SetBool("Falling", _isFalling);
        }
    }

    void OnMove(InputValue movementValue)
    {
        print("input");
        if (IsInputBlockedExternally || _isInputBlockedInternally)
        {
            _movementVector =  Vector2.zero;
            _storedMovementVector = movementValue.Get<Vector2>();
        }
        else
        {
            _movementVector = movementValue.Get<Vector2>();            
        }
    }

    void MoveCharacter()
    {        
        if (!IsGameOver && !(IsInputBlockedExternally || _isInputBlockedInternally))
        {
            if(_storedMovementVector != Vector2.zero)
            {
                _movementVector = _storedMovementVector;
                _storedMovementVector = Vector2.zero;
            }
            Vector2 inputDirection =  _movementVector;
            
            if ((_characterController.isGrounded || !_isObeyingGravity) && !_isSuperJumping)
            {
                _isSuperFalling = false;
                _gravityVelocity = 0;
            }

            if (_isSuperJumping)
            {
                _isSuperJumping = false;
                float jumpVelocity = Mathf.Sqrt(-2 * Gravity * 15);
                _gravityVelocity = jumpVelocity;
            }

            if (_isSuperFalling)
            {
                inputDirection = new Vector2(0, 1);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                JumpCharacter();
            }

            if (inputDirection != Vector2.zero)
            {
                float targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.y) * Mathf.Rad2Deg + _cameraTransform.eulerAngles.y;
                transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref _turnSmoothVelocity, GetModifiedSmoothTime(TurnSmoothTime));
            }

            bool isRunning = (Input.GetKey(KeyCode.RightShift)|| Input.GetKey(KeyCode.LeftShift)) ? true:false ;
            float targetSpeed = ((isRunning) ? _runSpeed : _walkSpeed) * inputDirection.magnitude;
            _currentSpeed = Mathf.SmoothDamp(_currentSpeed, targetSpeed, ref _speedSmoothVelocity, GetModifiedSmoothTime(SpeedSmoothTime));

            _gravityVelocity += Time.deltaTime * ((_isSuperFalling) ? Gravity/2 : Gravity);

            Vector3 movement = transform.forward * ((_isSuperFalling) ? 30 : _currentSpeed) + Vector3.up * _gravityVelocity;
        
            _characterController.Move(movement * Time.deltaTime);
            _currentSpeed = new Vector2(_characterController.velocity.x, _characterController.velocity.z).magnitude;

            float animationSpeedPercent = ((isRunning) ? _currentSpeed / _runSpeed : (_currentSpeed / _walkSpeed) * 0.5f);
            _animator.SetFloat("VelocityZ", animationSpeedPercent, SpeedSmoothTime, Time.deltaTime);            
        }
        //float velocityZ = Vector3.Dot(movement, transform.forward); //Useful for future feature: Target Lock
        //float velocityX = Vector3.Dot(movement, transform.right); //Useful for future feature: Target Lock
    }

    void JumpCharacter()
    {
        if (_characterController.isGrounded)
        {
            float jumpVelocity = Mathf.Sqrt(-2 * Gravity * JumpHeight);

            _gravityVelocity = jumpVelocity;
        }
    }

    private void PlayJumpSound()
    {
        _audioSource.clip = _jumpSound;
        _audioSource.Play();
    }

    public void SuperJumpCharacter()
    {
        _isSuperJumping = true;
        _isSuperFalling = true;
    }

    float GetModifiedSmoothTime(float smoothTime)
    {
        if (_characterController.isGrounded || !_isObeyingGravity || _isSuperFalling)
        {
            return smoothTime;
        }
        if (_airWalk == 0)
        {
            return float.MaxValue;
        }
        return smoothTime / _airWalk;
    }

    void CharacterAttack()
    {
        if (_isAttacking)
        {
            if (_animator.GetAnimatorTransitionInfo(0).duration == 0 && _animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1Windup"))
            {
                _animator.SetBool("Attack1Windup", false);
                _animator.SetBool("Attack1", true);                
            }
            if (_animator.GetAnimatorTransitionInfo(0).duration == 0 && _animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
            {
                _hurtbox.EnableHurtBoxCollider();
                _animator.SetBool("Attack1", false);
                _animator.SetBool("Attack1Recovery", true);
            }
            if (_animator.GetAnimatorTransitionInfo(0).duration == 0 && _animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1Recovery"))
            {
                _hurtbox.DisableHurtBoxCollider();
                _animator.SetBool("Attack1Recovery", false);
                _isInputBlockedInternally = false;
                _isAttacking = false;
            }
        }
    }

    void OnFire()
    {
        if (!IsGameOver && !(IsInputBlockedExternally || _isInputBlockedInternally || _isSuperFalling) && !_isAttacking)
        {
            _hurtbox.HasHurt = false;
            _isInputBlockedInternally = true;
            _isAttacking = true;
            float targetRotation = Mathf.Atan2(0, 1) * Mathf.Rad2Deg + _cameraTransform.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref _turnSmoothVelocity, 0);
           
            _animator.SetBool("Attack1Windup", true);
        }
    }    

    void Recover()
    {
        if (IsInputBlockedExternally && _animator.GetBool("TakeDamage") && _animator.GetCurrentAnimatorStateInfo(0).IsName("TakeDamage") && _animator.GetAnimatorTransitionInfo(0).duration == 0)
        {
            _animator.SetBool("TakeDamage", false);
            IsInputBlockedExternally = false;
            _isInputBlockedInternally = false;
        }
    }

    public void TakeDamageAnimation()
    {
        ExternalInterference();
        _animator.SetBool("TakeDamage", true);
    }

    void ExternalInterference()
    {
        IsInputBlockedExternally = true;
        CancelAttack();
    }

    void CancelAttack()
    {
        _animator.SetBool("Attack1Windup", false);
        _animator.SetBool("Attack1", false);
        _animator.SetBool("Attack1Recovery", false);
        _isAttacking = false;
    }
    public void DeathAnimation()
    {
        ExternalInterference();
        _animator.SetBool("Death", true);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "Unvisited")
        {
            IsNewIsland = true;
            NewIslandObject = hit.gameObject;
        }
        if (hit.gameObject.tag == "Visited" && IsNewIsland)
        {
            IsNewIsland = false;
        }
    } 
    void GetPlayerStats()
    {
        _walkSpeed = _playerStats.WalkSpeed;
        _runSpeed = _playerStats.RunSpeed;
        _airWalk = _playerStats.AirWalk;
    }

    void Step()
    {
        AudioClip clip = GetRandomClip();
        _audioSource.PlayOneShot(clip);
    }
    void NextStep()
    {
        AudioClip clip = GetRandomClip();
        _audioSource.PlayOneShot(clip);
    }

    AudioClip GetRandomClip()
    {
        int clipToGet = Random.Range(0, _footstepSounds.Length);
        return _footstepSounds[clipToGet];
    }

    void DeathRattle()
    {
        _audioSource.PlayOneShot(_deathSound);
    }

    void AttackSound()
    {
        _audioSource.PlayOneShot(_attackSound);
    }

    void HitSound()
    {
        _audioSource.PlayOneShot(_hitSound);
    }
}
