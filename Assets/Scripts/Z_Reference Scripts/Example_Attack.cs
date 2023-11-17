using UnityEngine;
using UnityEngine.InputSystem;

public class Example_Attack : MonoBehaviour
{
    Animator _animator;
    float _lastClickedTime;
    float _maximumComboDelay = 1;
    int _numberOfClicks = 0;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {        
    }
    
}
