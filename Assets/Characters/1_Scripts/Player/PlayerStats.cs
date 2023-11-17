
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerStats : MonoBehaviour
{
    public PlayerHurtbox Hurtbox;
    public PlayerCharacterController Controller;

    public int MaxHealth = 5;
    public int CurrentHealth;
    public int Stamina;
    public float WalkSpeed = 5;
    public float RunSpeed;
    public int Damage = 1;
    public int Score = 0;

    public UIDocument UIParent;
    float _lastStaminaUsedTime;
    float _deathTime = 0;

    [Range(0, 1)]
    public float AirWalk;

    PlayerUI _playerUI;

    private void Awake()
    {
        CurrentHealth = MaxHealth;
        RunSpeed = WalkSpeed * 2;
        Stamina = MaxHealth;
        AirWalk = 0;
        _playerUI = UIParent.GetComponent<PlayerUI>();
    }

    private void Update()
    {
        CheckIfPlayerIsDead();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "EnemyHurtbox")
        {
            print("ouch!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "EnemyHurtbox")
        {
            print(CurrentHealth);
        }
    }
    public void TakeDamage(int incomingDamage)
    {
        CurrentHealth -= incomingDamage;
        Controller.TakeDamageAnimation();
        _playerUI.DisplayCurrentHealth();
        if (CurrentHealth <= 0)
        {
            Controller.IsGameOver = true;
            Die();
        }
    }

    public void Die()
    {
        Controller.DeathAnimation();
        _playerUI.DisplayScore(Score.ToString());
    }

    public void GainMaxHealth(int maxHealthToGain)
    {
        MaxHealth += maxHealthToGain;
        CurrentHealth = (CurrentHealth + maxHealthToGain > MaxHealth) ? MaxHealth : CurrentHealth + maxHealthToGain;
        _playerUI.DisplayPowerup("Gained Max Health!");
        Score++;
    }

    public void GainHealth(int healthToGain)
    {
        CurrentHealth = (CurrentHealth + healthToGain > MaxHealth)? MaxHealth: CurrentHealth + healthToGain;
        _playerUI.DisplayPowerup("Regained " + healthToGain + " Health!");
        Score++;
    }

    public void GainSpeed(float speedToGain)
    {
        WalkSpeed += speedToGain;
        RunSpeed = WalkSpeed * 2;
        _playerUI.DisplayPowerup("Gained Speed!");
        Score++;
    }

    public void GainDamage(int damageToGain)
    {
        Damage += damageToGain;
        _playerUI.DisplayPowerup("Gained Damage!");
        Score++;
    }

    void CheckIfPlayerIsDead()
    {
        if (Controller.IsGameOver)
        {
            if (_deathTime == 0)
            {
                _deathTime = Time.time;
            }
            if (_deathTime != 0 && _deathTime + 3 < Time.time)
            {
                _playerUI.EnableDeathScreen();
            }
        }
    }

    public void Sprinting()
    {
        
    }

    public void RechargeStamina()
    {
        
    }
}
