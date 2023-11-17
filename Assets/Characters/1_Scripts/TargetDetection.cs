using UnityEngine;

public class TargetDetection : MonoBehaviour
{
    public Vector3 _targetPosition = Vector3.zero;
    public bool IsChasing = false;

    public Vector3 GetTargetPosition()
    {
        return _targetPosition;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            print("Found...you. " + _targetPosition + IsChasing);
            IsChasing = true;
            _targetPosition = other.gameObject.transform.position;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _targetPosition = other.gameObject.transform.position;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            print("Where...are...you?");
            IsChasing = false;
        }
    }
}
