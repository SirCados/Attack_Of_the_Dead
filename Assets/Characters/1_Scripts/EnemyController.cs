
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private AudioClip[] _footstepSounds;
    [SerializeField] private AudioClip _attackSound;
    [SerializeField] private AudioClip _hitSound;
    [SerializeField] private AudioClip _deathSound;
    [SerializeField] private AudioClip _foundYouSound;


    public bool IsChasing;
    public float Distance;
    public GameObject Eyes;

    private TargetDetection _targetDetector;
    private Vector3 _targetPosition;

    public bool IsInputBlockedExternally = false;
    public float MovementSpeed = 6;
    public float SpeedSmoothTime = 0.1f;
    public float TurnSmoothTime = 0.2f;
    public float Gravity = -9.8f;
    public float JumpHeight = 1;
    public GameObject Hurtbox;
    float AttackDistance = 1.5f;

    [Range(0, 1)]
    public float AirControlPercent;

    Animator _animator;
    AudioSource _audioSource;
    bool _isAttacking = false;
    CharacterController _characterController;
    CharacterHurtbox _hurtbox;
    MeshRenderer _hurtBoxVisualizer;
    float _gravityVelocity;

    // Start is called before the first frame update
    void Start()
    {
        _targetDetector = Eyes.GetComponent<TargetDetection>();
        _hurtbox = Hurtbox.GetComponent<CharacterHurtbox>();
        _hurtBoxVisualizer = Hurtbox.GetComponent<MeshRenderer>();
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
        _hurtBoxVisualizer = Hurtbox.GetComponent<MeshRenderer>();
        _hurtBoxVisualizer.enabled = false;
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Recover();
        if (!IsInputBlockedExternally && _targetDetector.IsChasing)
        {
            _targetPosition = _targetDetector.GetTargetPosition();            
        }
        else
        {
            _targetPosition = transform.forward;
        }

        CharacterAttack();        

        MoveCharacter();
    }    

    bool CheckIfTargetIsInRange()
    {
        return (Vector3.Distance(_targetPosition, transform.position) <= AttackDistance);
    }

    void MoveCharacter()
    {
        if (!(IsInputBlockedExternally || _isAttacking))
        {
            if (_targetDetector.IsChasing)
            {
                Vector3 targetDirection = _targetPosition - transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(new Vector3(targetDirection.x, 0f, targetDirection.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 100);
            }

            if (_characterController.isGrounded)
            {
                _gravityVelocity = 0;
            }

            _gravityVelocity += Time.deltaTime * Gravity;
            float targetSpeed = (_targetDetector.IsChasing && !CheckIfTargetIsInRange()) ? MovementSpeed : 0;

            Vector3 movement = transform.forward * targetSpeed + Vector3.up * _gravityVelocity;
            _characterController.Move(movement * Time.deltaTime);
            _animator.SetFloat("Velocity", targetSpeed, SpeedSmoothTime, Time.deltaTime);
        }        
    }

    void CharacterAttack()
    {
        if (!(IsInputBlockedExternally || _isAttacking) && CheckIfTargetIsInRange())
        {
            _isAttacking = true;
            _hurtbox.HasHurt = false;
            _animator.SetBool("Attack1Windup", true);
        }
        else if (!(IsInputBlockedExternally || _isAttacking) && !CheckIfTargetIsInRange())
        {
            _animator.SetBool("Attack1Windup", false);
        }
        else if (!IsInputBlockedExternally && _isAttacking)
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
                _animator.SetBool("Attack1Recovery", true);;
            }
            if (_animator.GetAnimatorTransitionInfo(0).duration == 0 && _animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1Recovery"))
            {
                _hurtbox.DisableHurtBoxCollider();
                _animator.SetBool("Attack1Recovery", false);
                _isAttacking = false;
            }
        }
    }

    void Recover()
    {
        if (IsInputBlockedExternally && _animator.GetBool("TakeDamage") && _animator.GetCurrentAnimatorStateInfo(0).IsName("TakeDamage") && _animator.GetAnimatorTransitionInfo(0).duration == 0)
        {
            _animator.SetBool("TakeDamage", false);            
            IsInputBlockedExternally = false;
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

    void FoundYouSound()
    {
        _audioSource.PlayOneShot(_foundYouSound);
    }
}
