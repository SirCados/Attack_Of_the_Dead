using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public CharacterHurtbox Hurtbox;
    public EnemyController Controller;
    public int _currentHealth = 5;
    public GameObject PowerUp;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerHurtbox")
        {
            print("ouch!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "PlayerHurtbox")
        {
            print(_currentHealth);
        }
    }

    public void TakeDamage(int incomingDamage)
    {
        _currentHealth -= incomingDamage;
        Controller.TakeDamageAnimation();
        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Controller.DeathAnimation();
        SpawnPowerUp();
    }

    void SpawnPowerUp()
    {
        Instantiate(PowerUp, transform.position,new Quaternion());
    }
}
