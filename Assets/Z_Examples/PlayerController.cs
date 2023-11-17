using UnityEngine;
using UnityEngine.InputSystem;

/*
C# naming conventions
private var _underscoreLowerCamelCase
public var UpperCamelCase
local/parameter var camelCase
 */

public class PlayerController : MonoBehaviour
{
    [SerializeField] float InputLagPeriod;
    [SerializeField] Vector2 Sensitivity;

    public bool IsGameOver = false;
    public float JumpForce = 10f;

    private Animator _animator;
    private AudioSource _audioSource;
    private bool _isOnGround = true;
    private float _inputLagTimer;
    private float _lookVectorX;
    private float _lookVectorY;
    private float _movementVectorX;
    private float _movementVectorY = 0f;
    private float _movementVectorZ;
    private GameObject _forward;
    private Rigidbody _playerRigidbody;
    private Vector2 _lastInputEvent;
    private Vector2 _movementVector;
    private Vector2 _rotation; //more stable than storing as the default quaternion    
    private Vector2 _velocity;

    private float _currentSpeed;

    void Start()
    {
        _playerRigidbody = GetComponent<Rigidbody>();
        _forward = GameObject.Find("Forward");
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    //Called just before any physics calculations
    //use when applying physics so that any physical interactions with the game object occurs at the same time as the physics engine
    //can occur never, once, or multiple times per frame, depending on the circumstance
    private void FixedUpdate()
    {
        RotatePlayer();
        if (_isOnGround && !IsGameOver)
        {
            if (Input.GetKey("e"))
            {
                _isOnGround = false;
                _playerRigidbody.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
            }
            else
            {
                _currentSpeed = _playerRigidbody.velocity.magnitude;
                Vector3 movement = new Vector3(_movementVectorX, _movementVectorY, _movementVectorZ);
                float velocityZ = Vector3.Dot(movement, transform.forward);
                float velocityX = Vector3.Dot(movement, transform.right);
                _animator.SetFloat("VelocityZ", velocityZ, 0.1f, Time.deltaTime);
                _animator.SetFloat("VelocityX", velocityX, 0.1f, Time.deltaTime);
            }
        }
    }

    void OnMove(InputValue movementValue)
    {
        _movementVector = movementValue.Get<Vector2>();
    }

    void OnLook(InputValue lookValue)
    {
        Vector2 lookVector = lookValue.Get<Vector2>();
        print(lookVector);

        _inputLagTimer += Time.deltaTime;
        

        //sometimes fast framerates will caust unity to not recieve inputs every frame. This addresses that.
        if ((Mathf.Approximately(0, lookVector.x) && Mathf.Approximately(0, lookVector.y)) == false || _inputLagTimer >= InputLagPeriod)
        {
            _lastInputEvent = lookVector;
            _inputLagTimer = 0;
        }
        _lookVectorX = _lastInputEvent.x;
        _lookVectorY = _lastInputEvent.y;
        
    }

    private void RotatePlayer()
    {
        Vector2 lookVelocity = new Vector2(_lookVectorX,_lookVectorY) * Sensitivity;
        Vector3 lookDirection = new Vector3(_lookVectorX, 0f, _lookVectorY);

        _velocity = new Vector2(
            Mathf.MoveTowards(_velocity.x, lookVelocity.x, Sensitivity.x * Time.deltaTime),
            Mathf.MoveTowards(_velocity.y, lookVelocity.y, -Sensitivity.y * Time.deltaTime));

        transform.forward -= lookDirection;
        print("New Forward: " + transform.forward);
        _rotation += _velocity * Time.deltaTime;
        //_rotation.y = ClampVerticalAngle(_rotation.y);
        transform.localEulerAngles = new Vector3(0, _rotation.x, 0);

        //else
        //{
        //    transform.localEulerAngles = new Vector3(_rotation.y, 0, 0);
        //}

        //if (IsOrientationUpdated() && IsMoveInput)
        //    UpdateOrientation();

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isOnGround = true;
            //dirtParticle.Play();
            //playerAudio.PlayOneShot(crashSound, .9f);
        }       
    }

    //void SetTargetRotation()
    //{
    //    // Create three variables, move input local to the player, flattened forward direction of the camera and a local target rotation.
    //    Vector2 moveInput = m_Input.MoveInput;
    //    Vector3 localMovementDirection = new Vector3(moveInput.x, 0f, moveInput.y).normalized;

    //    Vector3 forward = Quaternion.Euler(0f, cameraSettings.Current.m_XAxis.Value, 0f) * Vector3.forward;
    //    forward.y = 0f;
    //    forward.Normalize();

    //    Quaternion targetRotation;

    //    // If the local movement direction is the opposite of forward then the target rotation should be towards the camera.
    //    if (Mathf.Approximately(Vector3.Dot(localMovementDirection, Vector3.forward), -1.0f))
    //    {
    //        targetRotation = Quaternion.LookRotation(-forward);
    //    }
    //    else
    //    {
    //        // Otherwise the rotation should be the offset of the input from the camera's forward.
    //        Quaternion cameraToInputOffset = Quaternion.FromToRotation(Vector3.forward, localMovementDirection);
    //        targetRotation = Quaternion.LookRotation(cameraToInputOffset * forward);
    //    }

    //    // The desired forward direction of Ellen.
    //    Vector3 resultingForward = targetRotation * Vector3.forward;

    //    // Find the difference between the current rotation of the player and the desired rotation of the player in radians.
    //    float angleCurrent = Mathf.Atan2(transform.forward.x, transform.forward.z) * Mathf.Rad2Deg;
    //    float targetAngle = Mathf.Atan2(resultingForward.x, resultingForward.z) * Mathf.Rad2Deg;

    //    m_AngleDiff = Mathf.DeltaAngle(angleCurrent, targetAngle);
    //    m_TargetRotation = targetRotation;
    //}

    //void UpdateOrientation()
    //{
    //    _animator.SetFloat(m_HashAngleDeltaRad, m_AngleDiff * Mathf.Deg2Rad);

    //    Vector3 localInput = new Vector3(m_Input.MoveInput.x, 0f, m_Input.MoveInput.y);
    //    float groundedTurnSpeed = Mathf.Lerp(maxTurnSpeed, minTurnSpeed, m_ForwardSpeed / m_DesiredForwardSpeed);
    //    float actualTurnSpeed = m_IsGrounded ? groundedTurnSpeed : Vector3.Angle(transform.forward, localInput) * k_InverseOneEighty * k_AirborneTurnSpeedProportion * groundedTurnSpeed;
    //    m_TargetRotation = Quaternion.RotateTowards(transform.rotation, m_TargetRotation, actualTurnSpeed * Time.deltaTime);

    //    transform.rotation = m_TargetRotation;
    //}

    // Called each physics step.
    //void CalculateForwardMovement()
    //{
    //    // Calculate the speed intended by input.
    //    m_DesiredForwardSpeed = _movementVector.magnitude * maxForwardSpeed;

    //    // Determine change to speed based on whether there is currently any move input.
    //    float acceleration = IsMoveInput ? k_GroundAcceleration : k_GroundDeceleration;

    //    // Adjust the forward speed towards the desired speed.
    //    m_ForwardSpeed = Mathf.MoveTowards(m_ForwardSpeed, m_DesiredForwardSpeed, acceleration * Time.deltaTime);

    //    // Set the animator parameter to control what animation is being played.
    //    m_Animator.SetFloat(m_HashForwardSpeed, m_ForwardSpeed);
    //}

    //void OnAnimatorMove()
    //{
    //    Vector3 movement;

    //    // If Ellen is on the ground...
    //    if (m_IsGrounded)
    //    {
    //        // ... raycast into the ground...
    //        RaycastHit hit;
    //        Ray ray = new Ray(transform.position + Vector3.up * k_GroundedRayDistance * 0.5f, -Vector3.up);
    //        if (Physics.Raycast(ray, out hit, k_GroundedRayDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore))
    //        {
    //            // ... and get the movement of the root motion rotated to lie along the plane of the ground.
    //            movement = Vector3.ProjectOnPlane(m_Animator.deltaPosition, hit.normal);

    //            // Also store the current walking surface so the correct audio is played.
    //            Renderer groundRenderer = hit.collider.GetComponentInChildren<Renderer>();
    //            m_CurrentWalkingSurface = groundRenderer ? groundRenderer.sharedMaterial : null;
    //        }
    //        else
    //        {
    //            // If no ground is hit just get the movement as the root motion.
    //            // Theoretically this should rarely happen as when grounded the ray should always hit.
    //            movement = _animator.deltaPosition;
    //            m_CurrentWalkingSurface = null;
    //        }
    //    }
    //    else
    //    {
    //        // If not grounded the movement is just in the forward direction.
    //        movement = m_ForwardSpeed * transform.forward * Time.deltaTime;
    //    }

    //    // Rotate the transform of the character controller by the animation's root rotation.
    //    m_CharCtrl.transform.rotation *= _animator.deltaRotation;

    //    // Add to the movement with the calculated vertical speed.
    //    movement += m_VerticalSpeed * Vector3.up * Time.deltaTime;

    //    // Move the character controller.
    //    m_CharCtrl.Move(movement);

    //    // After the movement store whether or not the character controller is grounded.
    //    m_IsGrounded = m_CharCtrl.isGrounded;

    //    // If Ellen is not on the ground then send the vertical speed to the animator.
    //    // This is so the vertical speed is kept when landing so the correct landing animation is played.
    //    if (!m_IsGrounded)
    //        _animator.SetFloat(m_HashAirborneVerticalSpeed, m_VerticalSpeed);

    //    // Send whether or not Ellen is on the ground to the animator.
    //    _animator.SetBool(m_HashGrounded, m_IsGrounded);
    //}
}