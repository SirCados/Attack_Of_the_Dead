using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionExample : MonoBehaviour
{
    int _health = 5;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerHurtbox")
        {
            print("ouch!");
        }                
    }

    void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "PlayerHurtbox")
        {
            print("_health: " + _health);
        }
    }

    public void TakeDamage(int incomingDamage)
    {
        _health -= incomingDamage;
    }
}
