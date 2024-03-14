using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("Pause Options")]
    [SerializeField] private float volumeDecreaseDuringPause = 2f;

    private GameObject menuUI;

    private bool gamePaused = false;

    public void Awake()
    {
        menuUI = transform.GetChild(0).gameObject;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Pause")) Pause(!gamePaused);
    }

    public void Pause(bool pause)
    {
        gamePaused = pause;
        menuUI.SetActive(pause);
        if (pause)
        {
            Time.timeScale = 0f;
            AudioListener.volume /= volumeDecreaseDuringPause;
        }
        else 
        {
            Time.timeScale = 1f;
            AudioListener.volume *= volumeDecreaseDuringPause;
        }
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
