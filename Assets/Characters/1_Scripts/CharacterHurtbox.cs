using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHurtbox : MonoBehaviour
{
    public bool HasHurt = false;
    public bool IsDebugging = false;
    public int Damage = 1;

    // Start is called before the first frame update
    void Start()
    {
        DisableHurtBoxCollider();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            HasHurt = true;
            print("Gottem! Dealt " + Damage + " damage!");
            //probably could make generic object in future to grab damage
            PlayerStats target = other.gameObject.GetComponent<PlayerStats>();
            target.TakeDamage(Damage);
            DisableHurtBoxCollider();
        }
    }

    public void AddDamage(int newDamage)
    {
        Damage = newDamage;
    }

    public void EnableHurtBoxCollider()
    {
        if (!HasHurt)
        {
            gameObject.GetComponent<BoxCollider>().enabled = true;
            if (IsDebugging)
            {
                gameObject.GetComponent<MeshRenderer>().enabled = true;
            }            
        }
    }

    public void DisableHurtBoxCollider()
    {
        gameObject.GetComponent<BoxCollider>().enabled = false;
        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }
}
