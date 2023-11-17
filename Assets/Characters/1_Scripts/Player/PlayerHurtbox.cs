using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurtbox : MonoBehaviour
{

    public bool IsDebugging = false;
    public bool HasHurt = false;
    int _damage = 1;
    public GameObject Player;
    PlayerStats _playerStats;

    // Start is called before the first frame update
    void Start()
    {
        _playerStats = Player.GetComponent<PlayerStats>();
        DisableHurtBoxCollider();
    }

    // Update is called once per frame
    void Update()
    {
        if (HasHurt)
        {
            DisableHurtBoxCollider();
        }
        SetDamage();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            HasHurt = true;
            print("Gottem! Dealt " + _damage + " damage!");
            CharacterStats target = other.gameObject.GetComponent<CharacterStats>();            
            target.TakeDamage(_damage);
        }
    }

    void SetDamage()
    {
        if(_playerStats.Damage > _damage)
        {
            _damage = _playerStats.Damage;
            print("new dmage" + _playerStats.Damage);
        }      
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
