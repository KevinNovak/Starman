using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] GameObject deathEffects;
    [SerializeField] float levelLoadDelay = 1f;

    ScoreBoard scoreBoard;

    void Start()
    {
        scoreBoard = FindObjectOfType<ScoreBoard>();
    }

    void OnTriggerEnter(Collider other)
    {
        StartDeathSequence();
    }

    private void StartDeathSequence()
    {
        scoreBoard.active = false;
        SendMessage("OnDeath");
        deathEffects.SetActive(true);
        Invoke("ReloadScene", levelLoadDelay);
    }

    // Method name referenced by string
    private void ReloadScene()
    {
        var activeScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(activeScene);
    }
}
