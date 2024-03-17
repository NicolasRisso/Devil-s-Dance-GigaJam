using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("Pause Options")]
    [SerializeField] private float volumeDecreaseDuringPause = 2f;
    [SerializeField] private LayerMask musicLayer;

    private Dictionary<AudioSource, bool> audioSourcesStatus = new Dictionary<AudioSource, bool>();

    private GameObject menuUI;
    private AudioSource[] audioSources;


    private bool gamePaused = false;

    public void Awake()
    {
        menuUI = transform.GetChild(0).gameObject;
        audioSources = FindObjectsOfType<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Pause")) Pause(!gamePaused);
    }

    public void Pause(bool pause)
    {
        gamePaused = pause;
        menuUI.SetActive(pause);
        PauseAllAudios(pause);
        if (gamePaused)
        {
            Time.timeScale = 0f;
            AudioListener.volume /= volumeDecreaseDuringPause;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else 
        {
            Time.timeScale = 1f;
            AudioListener.volume *= volumeDecreaseDuringPause;
            if (AudioListener.volume > 1f) AudioListener.volume = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        AudioListener.volume *= volumeDecreaseDuringPause;
        SceneManager.LoadScene("MainMenu");
    }

    private void PauseAllAudios(bool value)
    {
        audioSources = FindObjectsOfType<AudioSource>();
        if (value)
        {
            audioSourcesStatus.Clear();
            foreach (AudioSource audioSource in audioSources)
            {
                if (audioSource is null) continue;
                audioSourcesStatus[audioSource] = audioSource.isPlaying;
                if (audioSource.gameObject.layer != 13) audioSource.Pause();
            }
        }
        else
        {
            foreach (KeyValuePair<AudioSource, bool> pair in audioSourcesStatus)
            {
                if (pair.Value)
                {
                    pair.Key.UnPause();
                }
            }
        }
    }
}
