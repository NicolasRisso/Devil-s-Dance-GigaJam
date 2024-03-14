using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Transition Configuration")]
    [SerializeField] private float timeToFadeBlack = 3f;

    private Animator darknessAnimator;
    private GameObject darkness;

    private void Start()
    {
        darkness = transform.parent.Find("Darkness").gameObject;
        darknessAnimator = darkness.GetComponent<Animator>();
    }

    private IEnumerator FadeToBlack()
    {
        darkness.SetActive(true);
        darknessAnimator.Play("Fadeout");
        yield return new WaitForSeconds(timeToFadeBlack);
        SceneManager.LoadScene("Farm");
    }

    private IEnumerator FadeOutAudioListenerVolume()
    {
        float startVolume = AudioListener.volume;

        while (AudioListener.volume > 0)
        {
            AudioListener.volume -= startVolume * Time.deltaTime / timeToFadeBlack;
            yield return null;
        }
    }

    public void PlayGame()
    {
        StartCoroutine(FadeToBlack());
        StartCoroutine(FadeOutAudioListenerVolume());
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
