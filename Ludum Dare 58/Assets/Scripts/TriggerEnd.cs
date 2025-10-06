using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TriggerEnd : MonoBehaviour
{
    [SerializeField] private GameObject winScreen;
    [SerializeField] private TextMeshProUGUI scoreText;

    private Player _player;
    private InputAction _restartAction;
    private bool _hasEnded;

    private void Awake()
    {
        _player = FindFirstObjectByType<Player>();
        _restartAction = InputSystem.actions.FindAction("Jump", true);
    }
    
    private void Update()
    {
        if (!_hasEnded)
        {
            return;
        }

        if (_restartAction.WasPressedThisFrame())
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (_hasEnded)
        {
            return;
        }
        
        if (!other.CompareTag("Player"))
        {
            return;
        }

        scoreText.text = $"Your collected stash was worth {_player.Score} points";
        winScreen.SetActive(true);
        Time.timeScale = 0;
        _hasEnded = true;
    }
}
