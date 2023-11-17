using UnityEngine;
using UnityEngine.UIElements;

public class PlayerUI : MonoBehaviour
{
    public GameObject Player;
    public SceneManagerScript SceneManagerObject;

    public bool LockCursor = false;

    PlayerStats _playerStats;

    ProgressBar _healthBar;
    ProgressBar _staminaBar;
    ProgressBar _magicBar;

    VisualElement _gameOverScreen;

    Button _respawnButton;
    Button _exitButton;
    Label _powerUpLabel;
    Label _scoreLabel;

    float _displayTime = 0;

    private void OnEnable()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        _playerStats = Player.GetComponent<PlayerStats>();
        _healthBar = root.Q<ProgressBar>("HealthBar");
        _healthBar.title = "HEALTH";

        _gameOverScreen = root.Q<VisualElement>("GameOverContainer");
        _gameOverScreen.visible = false;

        _scoreLabel = root.Q<Label>("Score");

        _respawnButton = root.Q<Button>("Respawn");
        _respawnButton.clicked += () => SceneManagerObject.ReloadScene();

        _exitButton = root.Q<Button>("Exit");
        _exitButton.clicked += () => ExitGame();

        _powerUpLabel = root.Q<Label>("PowerUpDisplay");
        _powerUpLabel.visible = false;

        GetMaxHealth();
        _healthBar.lowValue = 0;
        DisplayCurrentHealth();

        //_staminaBar = root.Q<ProgressBar>("StaminaBar");
        //_staminaBar.lowValue = 0;
        //_staminaBar.SetEnabled(false);

        //_magicBar = root.Q<ProgressBar>("MagicBar");
        //_magicBar.lowValue = 0;
        //_magicBar.SetEnabled(false);
    }

    void LockCursorToScreen()
    {
        LockCursor = !LockCursor;
        if (LockCursor)
        {
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;
        }
        else
        {
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;
        }
    }

    private void Update()
    {
        DisablePowerUpDisplay();
    }

    public void DisplayCurrentHealth()
    {
        _healthBar.value = _playerStats.CurrentHealth;
        _healthBar.visible = (_healthBar.value == _healthBar.highValue || _healthBar.value <= 0)? false : true;
    }

    public void GetMaxHealth()
    {
        _healthBar.highValue = _playerStats.MaxHealth;
    }

    public void EnableDeathScreen()
    {
        _gameOverScreen.visible = true;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;
    }

    public void DisplayPowerup(string powerUpType)
    {
        _powerUpLabel.visible = true;
        _powerUpLabel.text = powerUpType;        
        _displayTime = Time.time;        
    }

    void DisablePowerUpDisplay()
    {
        if (_powerUpLabel.visible)
        {            
            if (_displayTime != 0 && _displayTime + 5 < Time.time)
            {
                _powerUpLabel.visible = false;
            }
        }
    }

    void ExitGame()
    {
        Application.Quit();
    }

    public void DisplayScore(string score)
    {
        _scoreLabel.text = "Your score was " + score;
    }
}
