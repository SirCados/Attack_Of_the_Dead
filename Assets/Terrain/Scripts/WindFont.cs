using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindFont : MonoBehaviour
{
    PlayerCharacterController _playerController;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            print("Up you go!");
            //probably could make generic object in future to grab damage
            _playerController = other.gameObject.GetComponent<PlayerCharacterController>();
            SendUp();
        }
    }

    void SendUp()
    {
        _playerController.SuperJumpCharacter();
    }
}
