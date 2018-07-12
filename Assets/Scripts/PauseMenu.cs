using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class PauseMenu : MonoBehaviour
{
    public static bool paused = false;
    public GameObject menu;
    public Button initialSelect;
    public GameObject musicPlayer;

    private AudioSource audioSource;

    void Start()
    {
        musicPlayer = GameObject.FindGameObjectWithTag("Music");
        audioSource = musicPlayer.GetComponent<AudioSource>();
        Cursor.visible = false;
    }

    void Update()
    {
        if (CrossPlatformInputManager.GetButtonDown("Start"))
        {
            if (paused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    private void Pause()
    {
        Time.timeScale = 0.001f;
        audioSource.volume = 0.50f;
        menu.SetActive(true);
        Cursor.visible = true;

        EventSystem.current.SetSelectedGameObject(initialSelect.gameObject);

        paused = true;
    }

    public void Resume()
    {
        menu.SetActive(false);
        audioSource.volume = 1.0f;
        Time.timeScale = 1f;
        Cursor.visible = false;

        paused = false;
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1.0f;
        Destroy(musicPlayer);

        paused = false;

        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
