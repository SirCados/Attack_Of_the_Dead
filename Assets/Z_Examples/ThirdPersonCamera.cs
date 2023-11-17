using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField] float DistanceFromCameraTarget = 5;
    [SerializeField] float MouseSensitivity = 10;
    [SerializeField] float RotationSmoothTime = .12f;
    [SerializeField] Vector2 CameraVerticalClamp = new Vector2(-40, 80);

    public Transform CameraTarget;   

    float _pitch;
    float _yaw;
    Transform _lookTransform;
    Vector3 _rotationSmoothVelocity;
    Vector3 _currentRotation;

    private void Start()
    {
        _lookTransform = transform;
    }

    // LateUpdate is called after Update
    void LateUpdate()
    {
        MoveCamera();
    }

    void MoveCamera()
    {
        _yaw += Input.GetAxis("Mouse X") * MouseSensitivity;
        _pitch -= Input.GetAxis("Mouse Y") * MouseSensitivity;
        _pitch = Mathf.Clamp(_pitch, CameraVerticalClamp.x, CameraVerticalClamp.y);

        Vector3 targetRotation = new Vector3(_pitch, _yaw);

        _currentRotation = Vector3.SmoothDamp(_currentRotation, targetRotation, ref _rotationSmoothVelocity, RotationSmoothTime);

        _lookTransform.eulerAngles = _currentRotation;

        _lookTransform.position = CameraTarget.position - transform.forward * DistanceFromCameraTarget;
    }
}
