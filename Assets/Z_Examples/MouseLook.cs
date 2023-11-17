using UnityEngine;
using UnityEngine.InputSystem;

/*
C# naming conventions
private var _underscoreLowerCamelCase
public/serialized var UpperCamelCase
local/parameter var camelCase
 */

public class MouseLook : MonoBehaviour
{
    [SerializeField] private Vector2 Sensitivity;
    [SerializeField] private float InputLagPeriod;
    [SerializeField] private float MaxVerticalAngleFromHorizon;
 
    private Vector2 _velocity;
    private Vector2 _rotation; //more stable that storing as the default quaternion
    private Vector2 _lastInputEvent;
    private float _inputLagTimer;
    private bool _isPlayer = false;

    void Start()
    {
        if (gameObject.CompareTag("Player"))
        {
            _isPlayer = true;
        }
    }

    // Update is called once per frame
    //Called just before any physics calculations
    //use when applying physics so that any physical interactions with the game object occurs at the same time as the physics engine
    //can occur never, once, or multiple times per frame, depending on the circumstance
    private void Update()
    {
        RotatePlayer();
    }

    //can replace with new input system, but why? Safer?
    private Vector2 GetInput()
    {
        _inputLagTimer += Time.deltaTime;
        Vector2 input = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        //sometimes fast framerates will caust unity to not recieve inputs every frame. This addresses that.
        if ((Mathf.Approximately(0, input.x) && Mathf.Approximately(0, input.y)) == false || _inputLagTimer >= InputLagPeriod)
        {
            _lastInputEvent = input;
            _inputLagTimer = 0;
        }
        return _lastInputEvent;
    }

    private void RotatePlayer()
    {
        Vector2 lookVelocity = GetInput() * Sensitivity;

        _velocity = new Vector2(
            Mathf.MoveTowards(_velocity.x, lookVelocity.x, Sensitivity.x * Time.deltaTime),
            Mathf.MoveTowards(_velocity.y, lookVelocity.y, -Sensitivity.y * Time.deltaTime));

        _rotation += _velocity * Time.deltaTime;
        _rotation.y = ClampVerticalAngle(_rotation.y);
        if (_isPlayer)
        {
            transform.localEulerAngles = new Vector3(0, _rotation.x, 0);
        }
        else
        {
            transform.localEulerAngles = new Vector3(_rotation.y, 0, 0);
        }
    }

    void OnLook(InputValue movementValue)
    {

    }

    private float ClampVerticalAngle(float angle)
    {
        return Mathf.Clamp(angle, -MaxVerticalAngleFromHorizon, MaxVerticalAngleFromHorizon);
    }
}
