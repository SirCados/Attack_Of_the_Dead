using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerExample : MonoBehaviour
{
    public float WalkSpeed = 4;
    public float RunSpeed = 9;
    public float SpeedSmoothTime = 0.1f;
    public float TurnSmoothTime = 0.2f;

    float _currentSpeed;
    float _speedSmoothVelocity;
    float _turnSmoothVelocity;
    Vector2 _movementVector;
    Animator _animator;

    Transform _cameraTransform;    

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _cameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        _movementVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 inputDirection = _movementVector.normalized;

        if (inputDirection != Vector2.zero)
        {
            float targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.y) * Mathf.Rad2Deg + _cameraTransform.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref _turnSmoothVelocity, TurnSmoothTime);
        }

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float targetSpeed = ((isRunning) ? RunSpeed : WalkSpeed) * inputDirection.magnitude;
        _currentSpeed = Mathf.SmoothDamp(_currentSpeed, targetSpeed, ref _speedSmoothVelocity, SpeedSmoothTime);

        transform.Translate(transform.forward * _currentSpeed * Time.deltaTime, Space.World);

        float animationSpeedPercent = (isRunning) ? 1 : 0.5f * inputDirection.magnitude;
        //Animator.SetFloat("SpeedPercent", animationSpeedPercent, speedSmoothTime, Time.DeltaTime); //set up run forward animations and activate this

    }
}
