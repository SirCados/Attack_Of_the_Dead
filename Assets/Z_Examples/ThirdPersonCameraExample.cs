using UnityEngine;

public class ThirdPersonCameraExample : MonoBehaviour
{
    public bool LockCursor = true;
    public float MouseSensitivity = 10;
    public Transform CameraTarget;
    public float DistanceFromCameraTarget = 5;
    public Vector2 CameraVerticalClamp = new Vector2(-40, 80);

    public float RotationSmoothTime = .12f;
    Vector3 _rotationSmoothVelocity;
    Vector3 _currentRotation;

    float _pitch;
    float _yaw;
    private void Start()
    {
        if (LockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    // LateUpdate is called after Update
    void LateUpdate()
    {
        _yaw += Input.GetAxis("Mouse X") * MouseSensitivity;
        _pitch -= Input.GetAxis("Mouse Y") * MouseSensitivity;
        _pitch = Mathf.Clamp(_pitch, CameraVerticalClamp.x, CameraVerticalClamp.y);

        Vector3 targetRotation = new Vector3(_pitch, _yaw);

        _currentRotation = Vector3.SmoothDamp(_currentRotation, targetRotation, ref _rotationSmoothVelocity, RotationSmoothTime);
        
        transform.eulerAngles = _currentRotation;

        transform.position = CameraTarget.position - transform.forward * DistanceFromCameraTarget;
    }
}
