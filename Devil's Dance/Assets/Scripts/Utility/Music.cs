using System.Collections;
using UnityEngine;

public class Music : MonoBehaviour
{
    [SerializeField] private AudioSource ambienteSource;
    [SerializeField] private AudioSource persegui��oSource;
    [SerializeField] private float maxVolumeAmbient;
    [SerializeField] private float maxVolumeChase;
    [SerializeField] private float crossfadeTime = 2.0f;

    public void StartChaseMusic()
    {
        StartCoroutine(CrossfadeToPersegui��o());
    }

    private IEnumerator CrossfadeToPersegui��o()
    {
        float currentTime = 0;

        persegui��oSource.Play();

        while (currentTime < crossfadeTime)
        {
            ambienteSource.volume = Mathf.Lerp(maxVolumeAmbient, 0, currentTime / crossfadeTime);
            persegui��oSource.volume = Mathf.Lerp(0, maxVolumeChase, currentTime / crossfadeTime);

            currentTime += Time.deltaTime;
            yield return null;
        }

        ambienteSource.Stop();
    }

    public void StartAmbientMusic()
    {
        StartCoroutine(CrossfadeToAmbientacao());
    }

    private IEnumerator CrossfadeToAmbientacao()
    {
        float currentTime = 0;
        ambienteSource.Play();

        while (currentTime < crossfadeTime)
        {
            ambienteSource.volume = Mathf.Lerp(0, 1, currentTime / crossfadeTime);
            persegui��oSource.volume = Mathf.Lerp(1, 0, currentTime / crossfadeTime);

            currentTime += Time.deltaTime;
            yield return null;
        }
        persegui��oSource.Stop();
    }
}